/// <summary>
/// An <c>Interaction</c> is a data object used to replicate a player interaction in later loops
/// </summary>
public class Interaction
{
    private Interactable interactable;
    private float time;
    private ShadowMovement shadow;
    private bool done;

    public Interactable GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(Interactable value)
    {
        this.interactable = value;
    }

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float value)
    {
        this.time = value;
    }

    public ShadowMovement GetShadow()
    {
        return shadow;
    }

    public void SetShadow(ShadowMovement value)
    {
        this.shadow = value;
    }

    public bool GetDone()
    {
        return done;
    }

    public void SetDone(bool value)
    {
        this.done = value;
    }
}