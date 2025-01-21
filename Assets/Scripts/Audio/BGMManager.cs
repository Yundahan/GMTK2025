using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    private AudioSource bgmAudioSource;

    private Dictionary<string, string> sceneToBGMMapping = new Dictionary<string, string>
        {
          {"Scene1", "Sound/AhriTheme" },
          {"Scene2", "Sound/ACallToArmsTirionFordring"},
        };

    private BGMManager() {

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            bgmAudioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        this.transform.position = Camera.main.transform.position;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string activeSceneName = SceneLoader.Instance().GetActiveSceneName();

        if (!IsTrackCurrentlyPlaying(sceneToBGMMapping[activeSceneName]))
        {
            AudioClip clip = Resources.Load<AudioClip>(sceneToBGMMapping[activeSceneName]);
            bgmAudioSource.clip = clip;
            bgmAudioSource.Play();
        }
    }

    /// <summary>
    /// Changes the stored BGM file for a scene. If the scene is currently loaded, the BGM will be changed accordingly.
    /// </summary>
    /// <param name="sceneName">Name of the scene.</param>
    /// <param name="bgmFilePath">File path of the new BGM.</param>
    public void SetBGMForScene(string sceneName, string bgmFilePath)
    {
        sceneToBGMMapping[sceneName] = bgmFilePath;

        if (SceneLoader.Instance().GetActiveSceneName() == sceneName
            && !IsTrackCurrentlyPlaying(bgmFilePath))
        {
            bgmAudioSource.clip = Resources.Load<AudioClip>(bgmFilePath);
        }
    }

    /// <summary>
    /// Checks if the BGM from a given audio file is currently playing.
    /// </summary>
    /// <param name="bgmFilePath">Path of the BGM file.</param>
    public bool IsTrackCurrentlyPlaying(string bgmFilePath)
    {
        string[] pathArray = bgmFilePath.Split('/');
        string fileName = pathArray[pathArray.Length - 1];
        return bgmAudioSource.clip!= null && fileName == bgmAudioSource.clip.name;
    }

    public static BGMManager Instance()
    {
        if (instance == null)
        {
            instance = new BGMManager();
        }

        return instance;
    }
}