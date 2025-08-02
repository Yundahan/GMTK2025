using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public List<ToggleObject> toggleObjects;
    public Animator animator;

    private bool leverPulled = false;
    private bool active = false;

    public Sprite activeSprite;
    public Sprite inactiveSprite;

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
        UpdateSprite();
        leverPulled = true;
        SFXManager.Instance().PlaySFX("Lever");
        return true;
    }

    public bool GetLeverPulled()
    {
        return leverPulled;
    }

    protected void UpdateSprite()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().sprite = activeSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = inactiveSprite;
        }
    }
}
