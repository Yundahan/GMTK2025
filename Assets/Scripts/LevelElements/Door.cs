using System.Collections.Generic;
using UnityEngine;

public class Door : ToggleObject
{
    public Animator animator;
    public GameObject portalShine;

    private GameObject player;
    private SFXManager sfxManager;
    private Key[] allKeys;

    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        sfxManager = FindFirstObjectByType<SFXManager>();
        animator = GetComponent<Animator>();
        allKeys = FindObjectsByType<Key>(FindObjectsSortMode.None);
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
            Debug.Log("Tür ist offen und betreten");

            portalShine.SetActive(true);
            animator.SetBool("isClosing", true);
        } else
        {
            animator.SetBool("isClosing", false);
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
