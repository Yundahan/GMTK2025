using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowMovement : MonoBehaviour
{
    private Vector2 spawnPoint;
    private List<Vector2> path;

    public void ContinueOnPath(int newPathPosition)
    {
        if (newPathPosition < 0 || newPathPosition >= path.Count)
        {
            return;
        }

        float deltaX = path[newPathPosition].x - this.transform.position.x;

        if (deltaX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        } else if (deltaX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        this.transform.position = path[newPathPosition];
    }

    public void Reset()
    {
        this.transform.position = this.spawnPoint;
    }

    public void SetPath(List<Vector2> path)
    {
        this.path = path;
    }

    public void SetSpawnPoint(Vector2 value)
    {
        this.spawnPoint = value;
    }
}
