using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    private SFXManager sfxManager;
    public List<ToggleObject> toggleObjects;

    protected override void Awake()
    {
        base.Awake();
        sfxManager = FindFirstObjectByType<SFXManager>();
    }

    public override void Interact()
    {
        foreach (ToggleObject toggleObject in toggleObjects)
        {
            toggleObject.Toggle();
        }
        sfxManager.PlaySFX("Lever");
    }
    

}
