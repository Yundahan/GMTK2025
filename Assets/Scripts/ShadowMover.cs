using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMover : MonoBehaviour
{
    private List<Vector2> playerPath = new();
    private int pathPosition = 0;

    void FixedUpdate()
    {
        if (GetComponent<LoopManager>().GetLooping())
        {
            playerPath.Add(this.transform.position);
            MoveShadows();
        }
    }

    public void MoveShadows()
    {
        foreach (GameObject shadow in GetComponent<LoopManager>().GetShadows())
        {
            shadow.GetComponent<ShadowMovement>().ContinueOnPath(pathPosition);
        }

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
