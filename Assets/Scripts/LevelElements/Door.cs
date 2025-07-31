using Unity.VisualScripting;
using UnityEngine;

public class Door : ToggleObject
{
    private GameObject player;

    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<PlayerBehaviour>().gameObject;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {

        }
    }

    public override void Reset()
    {
        base.Reset();
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
