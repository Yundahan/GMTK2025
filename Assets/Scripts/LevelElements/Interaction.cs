using UnityEngine;
/// <summary>
/// An <c>Interaction</c> is a data object used to replicate a player interaction in later loops
/// </summary>
public class Interaction
{
    private Interactable interactable;
    private float time;
    private GameObject interacter;
    private bool done;

    // Property used for the throwing of keys only
    private Vector2 throwingDirection;

    public Interactable GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(Interactable value)
    {
        this.interactable = value;
    }

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float value)
    {
        this.time = value;
    }

    public GameObject GetInteracter()
    {
        return interacter;
    }

    public void SetInteracter(GameObject value)
    {
        this.interacter = value;
    }

    public bool GetDone()
    {
        return done;
    }

    public void SetDone(bool value)
    {
        this.done = value;
    }

    public Vector2 GetThrowingDirection()
    {
        return throwingDirection;
    }

    public void SetThrowingDirection(Vector2 value)
    {
        this.throwingDirection = value;
    }
}