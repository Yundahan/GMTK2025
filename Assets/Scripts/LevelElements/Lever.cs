using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public List<ToggleObject> toggleObjects;

    public Animator animator;

    public override void Interact()
    {
        foreach (ToggleObject toggleObject in toggleObjects)
        {
            toggleObject.Toggle();
        }
    }
}
