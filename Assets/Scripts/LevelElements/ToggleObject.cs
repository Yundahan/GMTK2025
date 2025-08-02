using UnityEngine;

public abstract class ToggleObject : MonoBehaviour
{
    public bool active = false;

    protected bool initValue = false;

    public Sprite activeSprite;
    public Sprite inactiveSprite;

    protected virtual void Awake()
    {
        initValue = active;
    }

    public void Toggle()
    {
        active = !active;
        ToggleActions();
    }

    protected abstract void ToggleActions();

    public virtual void Reset()
    {
        active = initValue;
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
