using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    private AudioSource[] bgmAudioSource;
    private SFXManager sfxManager;

    private Dictionary<string, List<string>> sceneToBGMMapping = new Dictionary<string, List<string>>
        {
          {"Scene2", new List<string> { "Sound/GTMK 2025 Testsoundtrack Layer1", "Sound/GTMK 2025 Testsoundtrack Layer2", "Sound/GTMK 2025 Testsoundtrack Layer3", "Sound/GTMK 2025 Testsoundtrack Layer4", "Sound/GTMK 2025 Testsoundtrack Layer5" } },
          {"Scene1", new List<string> { "Sound/GTMK 2025 Testsoundtrack Layer1", "Sound/GTMK 2025 Testsoundtrack Layer2", "Sound/GTMK 2025 Testsoundtrack Layer3", "Sound/GTMK 2025 Testsoundtrack Layer4", "Sound/GTMK 2025 Testsoundtrack Layer5" } },
          {"LeonTestScene", new List<string> { "Sound/GTMK 2025 Testsoundtrack Layer1", "Sound/GTMK 2025 Testsoundtrack Layer2", "Sound/GTMK 2025 Testsoundtrack Layer3", "Sound/GTMK 2025 Testsoundtrack Layer4", "Sound/GTMK 2025 Testsoundtrack Layer5" } }
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
            bgmAudioSource = GetComponents<AudioSource>();
            sfxManager = FindFirstObjectByType<SFXManager>();
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

    private void ShadowPlayLayers(string activeSceneName, int listlength)
    {
        for (int i = 1; i < listlength; i++)
        {
            AudioClip clip = Resources.Load<AudioClip>(sceneToBGMMapping[activeSceneName][i]);
            bgmAudioSource[i].clip = clip;
            bgmAudioSource[i].mute = true;
            bgmAudioSource[i].Play();
        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string activeSceneName = SceneLoader.Instance().GetActiveSceneName();

        if (!IsTrackCurrentlyPlaying(sceneToBGMMapping[activeSceneName][0]))
        {
            {
                AudioClip clip = Resources.Load<AudioClip>(sceneToBGMMapping[activeSceneName][0]);
                bgmAudioSource[0].clip = clip;
                bgmAudioSource[0].Play();
                ShadowPlayLayers(activeSceneName, sceneToBGMMapping[activeSceneName].Count);
            }
        }
        else
        {
            MuteShadowLayers();
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
        return bgmAudioSource[0].clip!= null && fileName == bgmAudioSource[0].clip.name;
    }

    public void ShadowSpawned(int shadowNumber)
    {
        if (shadowNumber < 0 || shadowNumber >= bgmAudioSource.Length)
        {
            return;
        }

        bgmAudioSource[shadowNumber].mute = false;
       
        sfxManager.PlaySFX("Loop");
        return;
    }
    private void MuteShadowLayers()
    {
        string activeSceneName = SceneLoader.Instance().GetActiveSceneName();

        for (int i = 1; i < bgmAudioSource.Length; i++)
        {
            AudioClip clip = Resources.Load<AudioClip>(sceneToBGMMapping[activeSceneName][i]);
            bgmAudioSource[i].clip = clip;
            bgmAudioSource[i].mute = true;
        }

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