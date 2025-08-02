using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerActions : Interacter
{
    private bool jumpBoosting = false;
    // actions from the current cycle, not yet to be repeated
    private List<Action> currentActions = new();
    // actions from previous cycles that are being repeated
    private List<Action> recordedActions = new();

    public Animator idleAnimator;
    public Animator movingAnimator;

    public void PerformPreviousActions(float loopStartTime)
    {
        foreach (Action action in recordedActions)
        {
            if (!action.GetDone() && Time.time - loopStartTime > action.GetTime())
            {
                action.GetShadow().PerformAction(action.GetActionType());
                action.SetDone(true);
            }
        }
    }

    public void EndLoop(ShadowActions shadow)
    {
        foreach (Action action in recordedActions)
        {
            action.SetDone(false);
            action.GetShadow().Reset();
        }

        // Add new shadow to current cycle interactions
        foreach (Action action in currentActions)
        {
            action.SetShadow(shadow);
        }

        // Add interactions from current cycle to complete list
        recordedActions.AddRange(currentActions);
        currentActions.Clear();
    }

    private void RecordAction(Action.ActionType actionType)
    {
        Action action = new Action();
        action.SetActionType(actionType);
        action.SetTime(Time.time - GetComponent<LoopManager>().GetLoopStartTime());
        currentActions.Add(action);
    }

    public List<Action> GetRecordedActions()
    {
        return recordedActions;
    }

    public bool GetJumpBoosting()
    {
        return jumpBoosting;
    }

    public void ActivateJumpBoosting()
    {
        if (GetComponent<PlayerMovement>().IsGrounded() && !jumpBoosting)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            this.jumpBoosting = true;
            RecordAction(Action.ActionType.JUMP_BOOSTING_ON);
            idleAnimator.SetBool("isJumpBoosting", true);
        }
    }

    public void DeactivateJumpBoosting()
    {

        idleAnimator.SetBool("isJumpBoosting", false);
        GetComponent<SpriteRenderer>().color = Color.white;
        this.jumpBoosting = false;
        RecordAction(Action.ActionType.JUMP_BOOSTING_OFF);
    }
}
