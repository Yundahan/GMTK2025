using UnityEngine;

public class ShadowActions : Interacter
{

    public Animator idleAnimator;
    public Animator movingAnimator;

    private bool isGrounded = true;
    private bool jumpBoosting = false;

    void Update()
    {
        Vector2 lastDirection = GetComponent<ShadowMovement>().GetLastDirection();

        if (!isGrounded)
        {
            if (lastDirection.y > 0f)
            {
                // JUMPING
            } else
            {
                // FALLING
            }
        } else if (!jumpBoosting)
        {
            if (Mathf.Abs(lastDirection.x) > 0.01f)
            {
                // MOVING
            } else
            {
                // IDLE
            }
        }
    }

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
            case Action.ActionType.JUMPING:
                isGrounded = false;
                break;
            case Action.ActionType.LANDING:
                isGrounded = true;
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
