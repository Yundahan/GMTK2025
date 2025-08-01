using UnityEngine;

public class ShadowActions : Interacter
{
    private bool jumpBoosting = false;

    public void PerformAction(Action.ActionType actionType)
    {
        switch (actionType)
        {
            case Action.ActionType.JUMP_BOOSTING_ON:
                ActivateJumpBoosting();
                break;
            case Action.ActionType.JUMP_BOOSTING_OFF:
                DeactivateJumpBoosting();
                break;
            default:
                break;
        }
    }

    public bool GetJumpBoosting()
    {
        return jumpBoosting;
    }

    public void ActivateJumpBoosting()
    {
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, 0.32f);
        this.jumpBoosting = true;
    }

    public void DeactivateJumpBoosting()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.32f);
        this.jumpBoosting = false;
    }

    public void Reset()
    {
        this.jumpBoosting = false;
    }
}
