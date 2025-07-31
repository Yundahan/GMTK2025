using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShadowManager : MonoBehaviour
{
    // PlayerShadow prefab for instantiating of new shadows
    public GameObject playerShadow;

    public float loopTime = 5f;

    // time in seconds between points on the recorded path of a shadow
    private const float PATH_INTERVAL = 0.01f;

    private List<ShadowMovement> shadows = new();
    private List<Vector2> playerPath = new();
    // interactions from the current cycle, not yet to be repeated
    private List<Interaction> currentInteractions = new();
    // interactions from previous cycles that are being repeated
    private List<Interaction> interactions = new();
    private ToggleObject[] toggleObjectsInScene;
    private Vector2 spawnPoint;
    private bool looping = false;
    private float loopStartTime = -5000f;
    private float lastPositionTime = -5000f;
    private int pathPosition = 0;

    private void Awake()
    {
        SetLooping(false);
        toggleObjectsInScene = FindObjectsByType<ToggleObject>(FindObjectsSortMode.None);
        spawnPoint = transform.position;
    }

    void Update()
    {
        if (looping)
        {
            PerformInteractions();

            if (Time.time - lastPositionTime > PATH_INTERVAL)
            {
                MoveShadows();
            }

            if (Time.time - loopStartTime > loopTime)
            {
                EndLoop();
            }
        }
    }

    private void PerformInteractions()
    {
        foreach (Interaction interaction in interactions)
        {
            if (!interaction.GetDone() && Time.time - loopStartTime > interaction.GetTime())
            {
                interaction.GetInteractable().Interact();
                interaction.SetDone(true);
            }
        }
    }

    private void MoveShadows()
    {
        playerPath.Add(GetComponent<PlayerMovement>().gameObject.transform.position);

        foreach (ShadowMovement shadowMovement in shadows)
        {
            shadowMovement.ContinueOnPath(pathPosition);
        }

        lastPositionTime = Time.time;
        pathPosition++;
    }

    private void EndLoop()
    {
        GetComponent<PlayerMovement>().Reset();

        foreach (ShadowMovement oldShadow in shadows)
        {
            oldShadow.Reset();
        }

        foreach (Interaction interaction in interactions)
        {
            interaction.SetDone(false);
            interaction.GetInteractable().Reset();
        }

        foreach (ToggleObject toggleObject in toggleObjectsInScene)
        {
            toggleObject.Reset();
        }

        // Add shadows
        GameObject newShadow = Instantiate(this.playerShadow, this.spawnPoint, Quaternion.identity);
        ShadowMovement newShadowMovement = newShadow.GetComponent<ShadowMovement>();
        newShadowMovement.SetSpawnPoint(spawnPoint);
        newShadowMovement.SetPath(playerPath);
        shadows.Add(newShadowMovement);

        // Add new shadow to current cycle interactions
        foreach (Interaction interaction in currentInteractions)
        {
            interaction.SetShadow(newShadowMovement);
        }

        // Add interactions from current cycle to complete list
        interactions.AddRange(currentInteractions);
        currentInteractions.Clear();

        BGMManager.Instance().ShadowSpawned(shadows.Count);
        playerPath = new();
        pathPosition = 0;
        loopStartTime = Time.time;
    }

    public void RecordInteraction(Interactable interactable)
    {
        Interaction interaction = new Interaction();
        interaction.SetInteractable(interactable);
        interaction.SetTime(Time.time - loopStartTime);
        currentInteractions.Add(interaction);
    }

    public void SetLooping(bool value)
    {
        // if looping is set to true and it wasnt before, set the times correctly
        if (value != looping && value)
        {
            playerPath = new();
            shadows.Clear();
            lastPositionTime = Time.time;
            loopStartTime = Time.time;
        }

        looping = value;
    }

    public bool GetLooping()
    {
        return looping;
    }
}
