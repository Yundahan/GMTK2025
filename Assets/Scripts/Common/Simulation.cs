using System.Collections;
using UnityEngine;

public class Simulation
{
    private static Simulation instance;

    // This is a variable that persists even when the scene changes.
    private int eCount = 0;

    private Simulation()
    {

    }

    public int IncreaseECount()
    {
        eCount++;
        return eCount;
    }

    /// <summary>
    /// Pauses the simulation if previously unpaused, unpauses the simulation if previously paused.
    /// Only those actions are paused which depend on Time.deltaTime!
    /// </summary>
    public void ToggleSimulation()
    {
        Time.timeScale = 1f -Time.timeScale;
    }

    /// <summary>
    /// Resets the current scene.
    /// </summary>
    public void Reset()
    {
        SceneLoader.Instance().ReloadScene();
    }

    public static Simulation Instance()
    {
        if (instance == null)
        {
            instance = new Simulation();
        }

        return instance;
    }
}
