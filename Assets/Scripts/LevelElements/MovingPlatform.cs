using UnityEngine;

public class MovingPlatform : Platform
{
    void FixedUpdate()
    {
        if (active && Vector3.Distance(this.transform.position, goalPosition) > 0.01f)
        {
            MoveWithPlayerAndObjects(goalPosition);
        } else if (!active && Vector3.Distance(this.transform.position, initPosition) > 0.01f)
        {
            MoveWithPlayerAndObjects(initPosition);
        }
    }
}
