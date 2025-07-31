using UnityEngine;

public abstract class ToggleObject : MonoBehaviour
{
    public bool active = false;

    private bool initValue = false;

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
}
