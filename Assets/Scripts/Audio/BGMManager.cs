using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    private AudioSource[] bgmAudioSource;

    private Dictionary<string, List<string>> sceneToBGMMapping = new Dictionary<string, List<string>>
        {
          {"Scene2", new List<string> { "Sound/Basement1", "Sound/Basement2", "Sound/Basement3", "Sound/Basement4", "Sound/Basement5" } },
          {"Scene1", new List<string> { "Sound/Floor1", "Sound/Floor2", "Sound/Floor3", "Sound/Floor4", "Sound/Floor5" } },
          {"LeonTestScene", new List<string> { "Sound/Observatory1", "Sound/Observatory2", "Sound/Observatory3", "Sound/Observatory4", "Sound/Observatory5","Sound/Observatory6" } },
          {"AndrikTestScene", new List<string> { "Sound/Floor1", "Sound/Floor2", "Sound/Floor3", "Sound/Floor4", "Sound/Floor5" } },
          {"MovementTest", new List<string> { "Sound/Floor1", "Sound/Floor2", "Sound/Floor3", "Sound/Floor4", "Sound/Floor5" } },
          {"Tutorial1TEST", new List<string> { "Sound/Floor1", "Sound/Floor2", "Sound/Floor3", "Sound/Floor4", "Sound/Floor5" } },
          {"Tutorial2TEST", new List<string> { "Sound/Floor1", "Sound/Floor2", "Sound/Floor3", "Sound/Floor4", "Sound/Floor5" } },
          {"Tutorial3TEST", new List<string> { "Sound/Floor1", "Sound/Floor2", "Sound/Floor3", "Sound/Floor4", "Sound/Floor5" } },
          {"Tutorial4TEST", new List<string> { "Sound/Floor1", "Sound/Floor2", "Sound/Floor3", "Sound/Floor4", "Sound/Floor5" } },
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
        bgmAudioSource[shadowNumber].volume = 0f;
        bgmAudioSource[shadowNumber].mute = false;
        StartCoroutine(FadeAudioSource.StartFade(bgmAudioSource[shadowNumber], 1f, 1f));

        return;
    }
    private void MuteShadowLayers()
    {
        string activeSceneName = SceneLoader.Instance().GetActiveSceneName();

        for (int i = 1; i < sceneToBGMMapping[activeSceneName].Count; i++)
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