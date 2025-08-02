using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Door : ToggleObject
{
    public GameObject portalShine;
    public string nextLevel;

    private const float LEVEL_END_DELAY = 1f;

    private Animator animator;
    private GameObject player;
    private SFXManager sfxManager;
    private Key[] allKeys;
    private bool doorReached = false;
    private float doorReachedTime = -5000f;


    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        sfxManager = FindFirstObjectByType<SFXManager>();
        allKeys = FindObjectsByType<Key>(FindObjectsSortMode.None);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (doorReached && Time.time - doorReachedTime > LEVEL_END_DELAY)
        {
            SceneLoader.Instance().LoadScene(nextLevel);
        }
    }

    protected override void ToggleActions()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            sfxManager.PlaySFX("Portal");

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
            Time.timeScale = 0f;

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
