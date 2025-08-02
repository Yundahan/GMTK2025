using UnityEngine;

public class AlternatingPlatform : Platform
{
    public float pauseDuration = 1f;

    private float arrivalTimer = -5000f;
    private bool pausing;

    void FixedUpdate()
    {
        if (pausing)
        {
            if (Time.time - arrivalTimer > pauseDuration)
            {
                pausing = false;
                active = !active;
            } else
            {
                return;
            }
        }

        if (active)
        {
            if (Vector3.Distance(this.transform.position, goalPosition) > 0.01f)
            {
                MoveWithPlayerAndObjects(goalPosition);
            } else
            {
                pausing = true;
                arrivalTimer = Time.time;
            }
        } else
        {
            if (Vector3.Distance(this.transform.position, initPosition) > 0.01f)
            {
                MoveWithPlayerAndObjects(initPosition);
            }
            else
            {
                pausing = true;
                arrivalTimer = Time.time;
            }
        }
    }
}
