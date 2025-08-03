using UnityEngine;

public class AlternatingPlatform : Platform
{
    public float pauseDuration = 1f;

    private LoopManager loopManager;

    private float arrivalTimer = -5000f;
    private bool pausing;

    protected override void Awake() 
    { 
        base.Awake();
        loopManager = FindFirstObjectByType<LoopManager>();
    }

    void FixedUpdate()
    {
        // Don't start moving until the player starts
        if (!loopManager.GetLooping())
        {
            return;
        }

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
