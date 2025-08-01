using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public List<ToggleObject> toggleObjects;

    public override void Interact(Interaction interaction)
    {
        foreach (ToggleObject toggleObject in toggleObjects)
        {
            toggleObject.Toggle();
        }
    }
}
