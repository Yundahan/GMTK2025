using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShadowMover : MonoBehaviour
{
    // time in seconds between points on the recorded path of a shadow
    private const float PATH_INTERVAL = 0.01f;

    private List<Vector2> playerPath = new();
    private float lastPositionTime = -5000f;
    private int pathPosition = 0;

    void Update()
    {
        if (GetComponent<LoopManager>().GetLooping())
        {
            if (Time.time - lastPositionTime > PATH_INTERVAL)
            {
                playerPath.Add(this.transform.position);
                MoveShadows();
            }
        }
    }

    public void MoveShadows()
    {
        foreach (GameObject shadow in GetComponent<LoopManager>().GetShadows())
        {
            shadow.GetComponent<ShadowMovement>().ContinueOnPath(pathPosition);
        }

        lastPositionTime = Time.time;
        pathPosition++;
    }

    public void EndLoop(List<GameObject> shadows, GameObject newShadow)
    {
        // Reset old shadows and interactions
        foreach (GameObject shadow in shadows)
        {
            shadow.GetComponent<ShadowMovement>().Reset();
        }

        AddNewShadow(newShadow);

        // reset values
        playerPath = new();
        pathPosition = 0;
    }

    private void AddNewShadow(GameObject newShadow)
    {
        // Add new shadow
        ShadowMovement newShadowMovement = newShadow.GetComponent<ShadowMovement>();
        newShadowMovement.SetSpawnPoint(GetComponent<PlayerMovement>().GetSpawnPoint());
        newShadowMovement.SetPath(playerPath);
    }
}
