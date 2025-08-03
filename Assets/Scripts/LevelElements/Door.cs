using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Door : ToggleObject
{
    
    public string nextLevel;

    private const float LEVEL_END_DELAY = 0.6f;

    private Animator animator;
    private GameObject player;
    private GameObject Blub;
    public SpriteRenderer blub;
    private Key[] allKeys;
    private bool doorReached = false;
    private float doorReachedTime = -5000f;

    public GameObject levelEndAnim;

    protected override void Awake()
    {
        base.Awake();
        UpdateSprite();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        allKeys = FindObjectsByType<Key>(FindObjectsSortMode.None);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (doorReached && Time.unscaledTime - doorReachedTime > LEVEL_END_DELAY)
        {
            Simulation.Instance().ToggleSimulation();
            SceneLoader.Instance().LoadScene(nextLevel);
        }
    }

    protected override void ToggleActions()
    {
        if (active)
        {
            SFXManager.Instance().PlaySFX("Portal");
            GetComponent<SpriteRenderer>().sprite = activeSprite;
            blub.enabled = true;
        } else
        {
            GetComponent<SpriteRenderer>().sprite = inactiveSprite;
            blub.enabled = false;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.gameObject == player && AllKeysCollected())
        {
            animator.SetBool("isClosing", true);
            doorReached = true;
            doorReachedTime = Time.unscaledTime;
            Instantiate(levelEndAnim, Vector3.zero, Quaternion.identity);
            Simulation.Instance().ToggleSimulation();
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
        UpdateSprite();
        blub.enabled = false;
    }
}
