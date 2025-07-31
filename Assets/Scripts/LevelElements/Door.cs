using Unity.VisualScripting;
using UnityEngine;

public class Door : ToggleObject
{
    private GameObject player;

    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
    }

    protected override void ToggleActions()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
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
        }
    }

    public override void Reset()
    {
        base.Reset();
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
