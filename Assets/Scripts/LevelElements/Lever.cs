using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public List<ToggleObject> toggleObjects;
    public Animator animator;

    private bool leverPulled = false;
    private bool active = false;

    public Sprite onSprite;
    public Sprite offSprite;

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

        if (animator != null)
        {
            animator.SetBool("isPressed", active);
            animator.SetBool("isON", active);
        } else
        {
            UpdateSprite();
        }

        return true;
    }

    public bool GetLeverPulled()
    {
        return leverPulled;
    }

    public override void Reset()
    {

        active = false;
        UpdateSprite();

        if (animator != null)
        {
            animator.SetBool("isPressed", false);
            animator.SetBool("isON", false);
        }
    }

    private void UpdateSprite()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().sprite = onSprite;
        } else
        {
            GetComponent<SpriteRenderer>().sprite = offSprite;
        }
    }
}
