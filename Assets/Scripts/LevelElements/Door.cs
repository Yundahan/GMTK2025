using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Door : ToggleObject
{
    private GameObject player;

    public Animator animator;


    public GameObject portalShine;

    private SFXManager sfxManager;


    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        sfxManager = FindFirstObjectByType<SFXManager>();

        animator = GetComponent<Animator>();
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
        if (active && collision.gameObject == player)
        {
            Debug.Log("Tür ist offen und betreten");

            portalShine.SetActive(true);

            animator.SetBool("isClosing", true);

          
        } return;
        
           
        
    }

    public override void Reset()
    {
        base.Reset();
        GetComponent<SpriteRenderer>().color = Color.red;
    }

}
