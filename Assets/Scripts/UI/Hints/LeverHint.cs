using UnityEngine;

public class LeverHint : Hint
{
    public Lever lever;

    void Update()
    {
        if (lever.GetLeverPulled())
        {
            HideMessage();
        }
    }
}
