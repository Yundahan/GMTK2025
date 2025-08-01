using UnityEngine;

public class MovingPlatform : ToggleObject
{
    public Vector3 movingDirection;
    public float platformSpeed = 1.5f;

    private Vector3 initPosition;
    private Vector3 goalPosition;

    protected override void Awake()
    {
        base.Awake();
        initPosition = this.transform.position;
        goalPosition = this.transform.position + movingDirection;
    }

    void FixedUpdate()
    {
        if (active && Vector3.Distance(this.transform.position, goalPosition) > 0.01f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, goalPosition, Time.fixedDeltaTime * platformSpeed);
        } else if (!active && Vector3.Distance(this.transform.position, initPosition) > 0.01f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, initPosition, Time.fixedDeltaTime * platformSpeed);
        }
    }

    protected override void ToggleActions()
    {
        // No operation necessary
    }

    public override void Reset()
    {
        base.Reset();
        this.transform.position = initPosition;
    }
}
