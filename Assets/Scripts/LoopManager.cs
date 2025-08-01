using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    // PlayerShadow prefab for instantiating of new shadows
    public GameObject playerShadow;

    public float loopTime = 5f;
    public int maxShadows = 4;

    // interactions from the current cycle, not yet to be repeated
    private List<Interaction> currentInteractions = new();
    // interactions from previous cycles that are being repeated
    private List<Interaction> interactions = new();

    private List<GameObject> shadows = new();
    private ToggleObject[] toggleObjectsInScene;
    private bool looping = false;
    private float loopStartTime = -5000f;

    // Audio values
    private float loopEndSoundPredelay = 0.2f;
    private bool loopEndSoundPlayed = false;

    private void Awake()
    {
        SetLooping(false);
        toggleObjectsInScene = FindObjectsByType<ToggleObject>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (looping)
        {
            PerformPreviousInteractions();
            GetComponent<PlayerActions>().PerformPreviousActions(loopStartTime);

            if (!loopEndSoundPlayed && Time.time - loopStartTime > loopTime - loopEndSoundPredelay)
            {
                SFXManager.Instance().PlaySFX("Loop");
                loopEndSoundPlayed = true;
            }

            if (Time.time - loopStartTime > loopTime)
            {
                EndLoop();
            }
        }
    }

    private void PerformPreviousInteractions()
    {
        foreach (Interaction interaction in interactions)
        {
            if (!interaction.GetDone() && Time.time - loopStartTime > interaction.GetTime())
            {
                interaction.GetInteractable().Interact(interaction);
                interaction.SetDone(true);
            }
        }
    }

    private void EndLoop()
    {
        GetComponent<PlayerMovement>().Reset();

        foreach (ToggleObject toggleObject in toggleObjectsInScene)
        {
            toggleObject.Reset();
        }

        GameObject newShadow = AddNewShadow();
        shadows.Add(newShadow);
        GetComponent<ShadowMover>().EndLoop(shadows, newShadow);
        GetComponent<PlayerActions>().EndLoop(newShadow.GetComponent<ShadowActions>());

        if (shadows.Count > maxShadows)
        {
            GameObject oldestShadow = shadows[0];
            interactions.RemoveAll(interaction => interaction.GetInteracter() == oldestShadow);
            GetComponent<PlayerActions>().GetRecordedActions().RemoveAll(action => action.GetShadow() == oldestShadow.GetComponent<ShadowActions>());
            shadows.RemoveAt(0);
            Destroy(oldestShadow.gameObject);
        }

        // Update BGM, reset loopStartTime
        BGMManager.Instance().ShadowSpawned(shadows.Count);
        loopEndSoundPlayed = false;
        loopStartTime = Time.time;
    }

    private GameObject AddNewShadow()
    {
        // Add new shadow
        GameObject newShadow = Instantiate(this.playerShadow, GetComponent<PlayerMovement>().GetSpawnPoint(), Quaternion.identity);

        // Add new shadow to current cycle interactions
        foreach (Interaction interaction in currentInteractions)
        {
            interaction.SetInteracter(newShadow);
        }

        // Add interactions from current cycle to complete list
        interactions.AddRange(currentInteractions);
        currentInteractions.Clear();

        foreach (Interaction interaction in interactions)
        {
            interaction.SetDone(false);
            interaction.GetInteractable().Reset();
        }

        return newShadow;
    }

    public Interaction RecordInteraction(Interaction interaction)
    {
        interaction.SetTime(Time.time - loopStartTime);
        interaction.SetInteracter(this.gameObject);
        currentInteractions.Add(interaction);
        return interaction;
    }

    public void SetLooping(bool value)
    {
        // if looping is set to true and it wasnt before, set the times correctly
        if (value != looping && value)
        {
            shadows.Clear();
            loopStartTime = Time.time;
        }

        looping = value;
    }

    public bool GetLooping()
    {
        return looping;
    }

    public float GetLoopStartTime()
    {
        return loopStartTime;
    }

    public List<GameObject> GetShadows()
    {
        return shadows;
    }
}
