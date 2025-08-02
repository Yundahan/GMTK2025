using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    // PlayerShadow prefab for instantiating of new shadows
    public GameObject playerShadow;
    // Shadow Spawn Animation prefab
    public GameObject shadowSpawnAnimPrefab;
    // Shadow Despawn Animation prefab
    public GameObject shadowDespawnAnimPrefab;

    public float loopDuration = 5f;
    public int maxShadows = 4;
    public float shadowDespawnAnimPredelay = 0.3f;

    // interactions from the current cycle, not yet to be repeated
    private List<Interaction> currentInteractions = new();
    // interactions from previous cycles that are being repeated
    private List<Interaction> interactions = new();
    // list of interactions that can't be repeated anymore
    private List<Interaction> deleteInteractions = new();

    private UIManager uiManager;
    private List<GameObject> shadows = new();
    private ToggleObject[] toggleObjectsInScene;
    private GameObject lastShadowSpawnAnim;
    private bool looping = false;
    private float loopStartTime = -5000f;
    private bool shadowDespawnAnimationPlayed = false;

    // Audio values
    private float loopEndSoundPredelay = 0.2f;
    private bool loopEndSoundPlayed = false;

    private void Awake()
    {
        SetLooping(false);
        uiManager = FindFirstObjectByType<UIManager>();
        toggleObjectsInScene = FindObjectsByType<ToggleObject>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (looping)
        {
            DeleteInteractionsFromList();
            PerformPreviousInteractions();
            GetComponent<PlayerActions>().PerformPreviousActions(loopStartTime);


            // Start playing the Loop end sounds a little earlier so that it actually matches up
            if (!loopEndSoundPlayed && Time.time - loopStartTime > loopDuration - loopEndSoundPredelay)
            {
                BGMManager.Instance().ShadowSpawned(shadows.Count + 1);
                SFXManager.Instance().PlaySFX("Loop");
                loopEndSoundPlayed = true;
            }

            //Start playing the shadow despawn animation a little earlier
            if (!shadowDespawnAnimationPlayed && Time.time - loopStartTime > loopDuration - shadowDespawnAnimPredelay)
            {
                foreach (GameObject shadow in  shadows)
                {
                    shadow.GetComponent<ShadowMovement>().StartDespawnAnimation(shadowDespawnAnimPrefab);
                }

                shadowDespawnAnimationPlayed = true;
            }

            if (Time.time - loopStartTime > loopDuration)
            {
                EndLoop();
            }
        }
    }

    private void PerformPreviousInteractions()
    {
        // Attempt all interactions and remove those that failed to execute
        interactions.RemoveAll(interaction => !AttemptInteraction(interaction));
    }

    private void DeleteInteractionsFromList()
    {
        foreach (Interaction interaction in deleteInteractions)
        {
            interactions.Remove(interaction);
        }

        deleteInteractions.Clear();
    }

    public void AddInteractionToDeletionList(Interaction interaction)
    {
        deleteInteractions.Add(interaction);
    }

    private bool AttemptInteraction(Interaction interaction)
    {
        if (!interaction.GetDone() && Time.time - loopStartTime > interaction.GetTime())
        {
            interaction.SetDone(true);
            return interaction.GetInteractable().Interact(interaction);
        }

        return true;
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

        // Update UI, reset loopStartTime
        this.uiManager.Reset();
        loopEndSoundPlayed = false;
        shadowDespawnAnimationPlayed = false;
        loopStartTime = Time.time;
    }

    private GameObject AddNewShadow()
    {
        // Add new shadow
        GameObject newShadow = Instantiate(this.playerShadow, GetComponent<PlayerMovement>().GetSpawnPoint(), Quaternion.identity);

        if (lastShadowSpawnAnim != null)
        {
            Destroy(lastShadowSpawnAnim);
        }

        lastShadowSpawnAnim = Instantiate(this.shadowSpawnAnimPrefab, GetComponent<PlayerMovement>().GetSpawnPoint(), Quaternion.identity);

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

    public float GetLoopDuration()
    {
        return loopDuration;
    }
}
