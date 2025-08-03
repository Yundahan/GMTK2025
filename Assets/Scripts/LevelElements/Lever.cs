using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public List<ToggleObject> toggleObjects;
    public Animator animator;

    private bool leverPulled = false;
    private bool active = false;

    protected override void Awake()
    {
        base.Awake();
    }

    
    public override bool Interact(Interaction interaction)
    {
        foreach (ToggleObject toggleObject in toggleObjects)
        {
            toggleObject.Toggle();
        }

        active = !active;
        leverPulled = true;
        SFXManager.Instance().PlaySFX("Lever");
        animator.SetBool("isPressed",  active);
        animator.SetBool("isON", active);
        return true;
    }

    public bool GetLeverPulled()
    {
        return leverPulled;
    }

    public override void Reset()
    {

        active = false;
        animator.SetBool("isPressed", false);
        animator.SetBool("isON", false);

    }

    
}
