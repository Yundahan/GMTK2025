using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    private SFXManager sfxManager;
    public List<ToggleObject> toggleObjects;
    public Animator animator;

    protected override void Awake()
    {
        base.Awake();
        sfxManager = FindFirstObjectByType<SFXManager>();
    }

    
    public override bool Interact(Interaction interaction)
    {
        foreach (ToggleObject toggleObject in toggleObjects)
        {
            toggleObject.Toggle();
        }

        sfxManager.PlaySFX("Lever");
        return true;
    }
}
