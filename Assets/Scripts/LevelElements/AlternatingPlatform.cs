using UnityEngine;

public class AlternatingPlatform : ToggleObject
{
    public float pauseDuration = 1f;
    public Vector3 movingDirection;
    public float platformSpeed = 1.5f;

    private Vector3 initPosition;
    private Vector3 goalPosition;

    private float arrivalTimer = -5000f;
    private bool pausing;
    private PlayerMovement player;

    protected override void Awake()
    {
        base.Awake();
        initPosition = this.transform.position;
        goalPosition = this.transform.position + movingDirection;
        player = FindFirstObjectByType<PlayerMovement>();
    }

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
                Vector3 newPosition = Vector3.MoveTowards(this.transform.position, goalPosition, Time.fixedDeltaTime * platformSpeed);
                Vector3 delta = newPosition - this.transform.position;

                if (GetComponent<Collider2D>().IsTouching(player.GetFeet()))
                {
                    player.transform.position += delta;
                }

                this.transform.position = newPosition;
            } else
            {
                pausing = true;
                arrivalTimer = Time.time;
            }
        } else
        {
            if (Vector3.Distance(this.transform.position, initPosition) > 0.01f)
            {
                Vector3 newPosition = Vector3.MoveTowards(this.transform.position, initPosition, Time.fixedDeltaTime * platformSpeed);
                Vector3 delta = newPosition - this.transform.position;

                if (GetComponent<Collider2D>().IsTouching(player.GetFeet()))
                {
                    player.transform.position += delta;
                }

                this.transform.position = newPosition;
            }
            else
            {
                    pausing = true;
                    arrivalTimer = Time.time;
            }
        }
    }

    protected override void ToggleActions()
    {
        // No operation necessary
    }
}
