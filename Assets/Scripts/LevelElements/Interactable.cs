using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float cooldown = 1f;

    private GameObject player;

    private float lastInteractionTime = -5000f;

    protected virtual void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (InputManager.Instance().IsPlayerInteracting() 
            && collision.gameObject == player
            && Time.time - lastInteractionTime > cooldown)
        {
            Interact();
            player.GetComponent<LoopManager>().RecordInteraction(this);
            lastInteractionTime = Time.time;
        }
    }

    public abstract void Interact();

    public void Reset()
    {
        lastInteractionTime = -5000f;
    }
}
