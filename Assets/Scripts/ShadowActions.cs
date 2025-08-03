using UnityEditor.Experimental.GraphView;
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
            if (lastDirection.y > 0f && Mathf.Abs(lastDirection.x) > 0.01f)
            {
                movingAnimator.SetBool("isJumping", true);
                movingAnimator.SetBool("isFalling", false);
            }
            else
            {
                movingAnimator.SetBool("isFalling", true);
                movingAnimator.SetBool("isJumping", false);
            }
        }

        else if (!jumpBoosting)
        {
            if (Mathf.Abs(lastDirection.x) > 0.01f)
            {
                movingAnimator.SetBool("isRunning", true);
                // MOVING
            }
            else
            {
                movingAnimator.SetBool("isRunning", false);
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
