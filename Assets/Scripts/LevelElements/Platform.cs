using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : ToggleObject
{
    public List<GameObject> connectedObjects;
    public Vector3 movingDirection;
    public float platformSpeed = 1.5f;

    protected Vector3 goalPosition;

    private List<Vector3> positions = new();
    private PlayerMovement player;

    protected override void Awake()
    {
        base.Awake();
        initPosition = this.transform.position;
        goalPosition = this.transform.position + movingDirection;
        player = FindFirstObjectByType<PlayerMovement>();

        foreach (GameObject go in connectedObjects)
        {
            positions.Add(go.transform.position - this.transform.position);
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
        MoveConnectedObjects();
    }

    protected void MoveWithPlayerAndObjects(Vector3 goalPosition)
    {
        Vector3 newPosition = Vector3.MoveTowards(this.transform.position, goalPosition, Time.fixedDeltaTime * platformSpeed);
        Vector3 delta = newPosition - this.transform.position;

        if (GetComponent<Collider2D>().IsTouching(player.GetFeet()))
        {
            player.transform.position += delta;
        }

        MoveConnectedObjects();
        this.transform.position = newPosition;
    }

    private void MoveConnectedObjects()
    {
        for (int i = 0; i < connectedObjects.Count; i++)
        {
            GameObject go = connectedObjects[i];

            // Special case for keys that were picked up already
            Key key = go.GetComponent<Key>();

            if (key != null && key.GetHasBeenPickedUpAtLeastOnce())
            {
                continue;
            }

            go.transform.position = this.transform.position + positions[i];
        }
    }
}
