using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    private static SFXManager instance;

    private AudioSource[] sfxAudioSource;

    private Dictionary<string, string> sfxDict = new Dictionary<string, string>
    {
        {"Lever", "Sound/SFX/SFX_Lever"},
        {"Loop", "Sound/SFX/SFX_Loop"},
        {"Portal", "Sound/SFX/SFX_Portal_Open"},
        {"Jump", "Sound/SFX/SFX_Jump"},
        {"Landing", "Sound/SFX/SFX_Landing"},
        {"Key", "Sound/SFX/SFX_Key_Pickup"},
        {"Yeet", "Sound/SFX/SFX_Throw"},
        {"Button", "Sound/SFX/SFX_UI_Button_Pressed"},
        {"Level_Select", "Sound/SFX/SFX_UI_Select_Level" },
        {"Shadow", "Sound/SFX/SFX_Shadow_Disappear" }
    };

    private SFXManager() {

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sfxAudioSource = GetComponents<AudioSource>();
    }

    private void Update()
    {
        this.transform.position = Camera.main.transform.position;
    }

    public void PlaySFX(string name)
    {
        for (int i = 0; i < sfxAudioSource.Length; i++)
        {
            if (!sfxAudioSource[i].isPlaying)
            {
                AudioClip clip = Resources.Load<AudioClip>(sfxDict[name]);
                sfxAudioSource[i].clip = clip;
                sfxAudioSource[i].Play();
                return;
            }
        }
    }

    public static SFXManager Instance()
    {
        if (instance == null)
        {
            instance = new SFXManager();
        }

        return instance;
    }
}