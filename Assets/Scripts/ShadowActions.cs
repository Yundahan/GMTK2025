using UnityEngine;

public class ShadowActions : Interacter
{
    public Animator idleAnimator;
    public Animator movingAnimator;

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
        idleAnimator.SetBool("isJumpBoosting", true);
        this.jumpBoosting = true;
    }

    public void DeactivateJumpBoosting()
    {
        idleAnimator.SetBool("isJumpBoosting", false);
        this.jumpBoosting = false;
    }

    public void Reset()
    {
        this.jumpBoosting = false;
    }
}
