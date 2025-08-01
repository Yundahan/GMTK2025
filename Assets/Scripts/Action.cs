/// <summary>
/// An <c>Action</c> is a data object used to replicate a player action in later loops
/// </summary>
public class Action
{
    public enum ActionType
    {
        JUMP_BOOSTING_ON,
        JUMP_BOOSTING_OFF
    }

    private ActionType actionType;
    private float time;
    private ShadowActions shadow;
    private bool done;

    public ActionType GetActionType()
    {
        return actionType;
    }

    public void SetActionType(ActionType value)
    {
        this.actionType = value;
    }

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float value)
    {
        this.time = value;
    }

    public ShadowActions GetShadow()
    {
        return shadow;
    }

    public void SetShadow(ShadowActions value)
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