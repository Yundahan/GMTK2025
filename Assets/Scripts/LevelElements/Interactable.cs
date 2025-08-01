using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float cooldown = 1f;

    protected PlayerActions player;

    protected float lastInteractionTime = -5000f;

    protected virtual void Awake()
    {
        player = FindFirstObjectByType<PlayerActions>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (InputManager.Instance().IsPlayerInteracting()
            && collision == player.GetComponentInChildren<CircleCollider2D>()
            && Time.time - lastInteractionTime > cooldown)
        {
            Interaction interaction = RecordInteraction();
            Interact(interaction);
            lastInteractionTime = Time.time;
        }
    }

    public virtual Interaction RecordInteraction()
    {
        Interaction interaction = new Interaction();
        interaction.SetInteractable(this);
        interaction.SetInteracter(player.gameObject);
        player.GetComponent<LoopManager>().RecordInteraction(interaction);
        return interaction;
    }

    public abstract void Interact(Interaction interaction);

    public virtual void Reset()
    {
        lastInteractionTime = -5000f;
    }
}
