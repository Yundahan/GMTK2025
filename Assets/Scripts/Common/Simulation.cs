using System.Collections;
using UnityEngine;

public class Simulation
{
    private static Simulation instance;

    private bool simulating = true;

    private Simulation()
    {

    }

    /// <summary>
    /// Pauses the simulation if previously unpaused, unpauses the simulation if previously paused.
    /// Only those actions are paused which depend on Time.deltaTime!
    /// </summary>
    public void ToggleSimulation()
    {
        simulating = !simulating;
        Time.timeScale = 1f -Time.timeScale;
    }

    /// <summary>
    /// Returns whether the simulation is currently running or paused.
    /// </summary>
    public bool IsSimulating()
    {
        return simulating;
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
