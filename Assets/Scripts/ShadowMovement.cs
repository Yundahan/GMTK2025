using System.Collections.Generic;
using UnityEngine;

public class ShadowMovement : MonoBehaviour
{
    public Animator animator;

    private Vector2 spawnPoint;
    private List<Vector2> path;
    private GameObject despawnAnim;

    public void ContinueOnPath(int newPathPosition)
    {
        if (newPathPosition < 0 || newPathPosition >= path.Count)
        {
            return;
        }

        this.transform.position = path[newPathPosition];
    }

    public void StartDespawnAnimation(GameObject despawnPrefab)
    {
        // clean up old animation object
        if (despawnAnim != null)
        {
            Destroy(despawnAnim);
        }

        despawnAnim = Instantiate(despawnPrefab, this.transform);
    }

    public void Reset()
    {
        // leave despawn animation behind when resetting position
        if (despawnAnim != null)
        {
            despawnAnim.transform.parent = null;
        }

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
