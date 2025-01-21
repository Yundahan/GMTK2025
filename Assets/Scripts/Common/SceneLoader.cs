using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private static SceneLoader instance;

    private SceneLoader()
    {

    }

    /// <summary>
    /// Loads a new scene.
    /// </summary>
    /// <param name="sceneName">Name of the scene file that will be loaded.</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Loads a new scene.
    /// </summary>
    /// <param name="sceneBuildIndex">Build index of the scene file that will be loaded.</param>
    public void LoadScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    /// <summary>
    /// Reloads current scene.
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetActiveSceneBuildIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static SceneLoader Instance()
    {
        if (instance == null)
        {
            instance = new SceneLoader();
        }

        return instance;
    }
}
