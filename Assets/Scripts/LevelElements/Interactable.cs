using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float cooldown = 1f;

    protected PlayerActions player;

    protected float lastInteractionTime = -5000f;

    protected Vector2 initPosition;

    protected virtual void Awake()
    {
        initPosition = this.transform.position;
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

    public abstract bool Interact(Interaction interaction);

    public virtual void Reset()
    {
        this.transform.position = initPosition;
        lastInteractionTime = -5000f;
    }
}
