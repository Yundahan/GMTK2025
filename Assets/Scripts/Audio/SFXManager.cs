using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    private static SFXManager instance;

    private AudioSource[] sfxAudioSource;

    private Dictionary<string, string> sfxDict = new Dictionary<string, string>
    {
        {"Lever", "Sound/SFX/SFX_Lever" },
        {"Loop", "Sound/SFX/SFX_Loop" },
        {"Portal", "Sound/SFX/SFX_Portal_Open" }


    };


    private SFXManager() {

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            sfxAudioSource = GetComponents<AudioSource>();
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

    public void PlaySFX(string name)
    {
        for (int i = 0; i < sfxAudioSource.Length; i++)
        {
            if (!sfxAudioSource[i].isPlaying)
            {
                AudioClip clip = Resources.Load<AudioClip>(sfxDict[name]);
                sfxAudioSource[i].clip = clip;
                sfxAudioSource[i].Play();
                i = sfxAudioSource.Length;
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