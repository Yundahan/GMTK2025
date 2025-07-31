using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    private AudioSource[] bgmAudioSource;

    private Dictionary<string, List<string>> sceneToBGMMapping = new Dictionary<string, List<string>>
        {
          {"Scene2", new List<string> { "Sound/GTMK 2025 Testsoundtrack Layer1", "Sound/GTMK 2025 Testsoundtrack Layer2", "Sound/GTMK 2025 Testsoundtrack Layer3", "Sound/GTMK 2025 Testsoundtrack Layer4", "Sound/GTMK 2025 Testsoundtrack Layer5" } },
          {"Scene1", new List<string> { "AdditionalCardPersonAdressType" } },
        };

    private BGMManager() {

    }

    private void shadowPlayLayers(string activeSceneName, int listlength)
    {
        for (int i = 1; i < listlength; i++)
        {
            AudioClip clip = Resources.Load<AudioClip>(sceneToBGMMapping[activeSceneName][i]);
            bgmAudioSource[i].clip = clip;
            bgmAudioSource[i].mute = true;
            bgmAudioSource[i].Play();
        }

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            bgmAudioSource = GetComponents<AudioSource>();
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

        if (!IsTrackCurrentlyPlaying(sceneToBGMMapping[activeSceneName][0]))
        {
            {
                AudioClip clip = Resources.Load<AudioClip>(sceneToBGMMapping[activeSceneName][0]);
                bgmAudioSource[0].clip = clip;
                bgmAudioSource[0].Play();
                shadowPlayLayers(activeSceneName, sceneToBGMMapping[activeSceneName].Count);
            }
        }
        else
        {
            muteShadowLayers();
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

    public static BGMManager Instance()
    {
        if (instance == null)
        {
            instance = new BGMManager();
        }

        return instance;
    }

    public void shadowSpawned(int shadowNumber)
    {
        bgmAudioSource[shadowNumber].mute = false;
    }
    private void muteShadowLayers()
    {
        string activeSceneName = SceneLoader.Instance().GetActiveSceneName();

        for (int i = 1; i < bgmAudioSource.Length; i++)
        {
            AudioClip clip = Resources.Load<AudioClip>(sceneToBGMMapping[activeSceneName][i]);
            bgmAudioSource[i].clip = clip;
            bgmAudioSource[i].mute = true;
          
        }

    }



    
}