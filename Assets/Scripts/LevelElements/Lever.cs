using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public List<ToggleObject> toggleObjects;
    public Animator animator;

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

        SFXManager.Instance().PlaySFX("Lever");
        return true;
    }
}
