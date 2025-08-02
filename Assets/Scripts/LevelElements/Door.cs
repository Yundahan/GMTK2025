using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Door : ToggleObject
{
    public GameObject portalShine;
    public string nextLevel;

    private const float LEVEL_END_DELAY = 1f;

    private Animator animator;
    private GameObject player;
    private Key[] allKeys;
    private bool doorReached = false;
    private float doorReachedTime = -5000f;


    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        allKeys = FindObjectsByType<Key>(FindObjectsSortMode.None);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (doorReached && Time.time - doorReachedTime > LEVEL_END_DELAY)
        {
            Simulation.Instance().ToggleSimulation();
            SceneLoader.Instance().LoadScene(nextLevel);
        }
    }

    protected override void ToggleActions()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            SFXManager.Instance().PlaySFX("Portal");

        } else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.gameObject == player && AllKeysCollected())
        {
            portalShine.SetActive(true);
            animator.SetBool("isClosing", true);
            Simulation.Instance().ToggleSimulation();
            doorReached = true;
        }
    }

    private bool AllKeysCollected()
    {
        foreach (Key key in allKeys)
        {
            if (!key.IsInPlayerHand())
            {
                return false;
            }
        }

        return true;

    }

    public override void Reset()
    {
        base.Reset();
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
