using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    private static VolumeManager instance;

    public AudioMixer audioMixer;

    // These are variables that persist even when the scene changes.
    Dictionary<string, float> volumes = new Dictionary<string, float>
        {
          {"MasterVolume", 0.1f },
          {"BGMVolume", 1f},
          {"SFXVolume", 1f}
        };

    private VolumeManager()
    {

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        this.SetMasterVolume(this.volumes["MasterVolume"]);
    }

    public float GetMasterVolume()
    {
        return this.volumes["MasterVolume"];
    }

    /// <summary>
    /// Sets the master volume to a given value.
    /// </summary>
    /// <param name="masterVolume">The new master volume value.</param>
    public void SetMasterVolume(float masterVolume)
    {
        this.volumes["MasterVolume"] = masterVolume;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
    }

    /// <summary>
    /// Gets the volume for a volume type.
    /// </summary>
    /// <param name="volumeType">The volume type.</param>
    public float GetVolume(string volumeType)
    {
        return this.volumes[volumeType];
    }

    /// <summary>
    /// Sets a specific volume type to a given value.
    /// </summary>
    /// <param name="volumeType">The volume type.</param>
    /// <param name="volume">The new volume value.</param>
    public void SetVolume(string volumeType, float volume)
    {
        this.volumes[volumeType] = volume;
        audioMixer.SetFloat(volumeType, Mathf.Log10(volume) * 20);
    }

    /// <summary>
    /// If the given volume type is muted, set to previous non-muted value, otherwise mute it.
    /// </summary>
    /// <param name="volumeType">The volume type.</param>
    public void ToggleMute(string volumeType)
    {
        audioMixer.GetFloat(volumeType, out float volume);

        if (volume != -80)
        {
            audioMixer.SetFloat(volumeType, -80);
        }
        else
        {
            audioMixer.SetFloat(volumeType, Mathf.Log10(this.volumes[volumeType]) * 20);
        }
    }

    public static VolumeManager Instance()
    {
        if (instance == null)
        {
            instance = new VolumeManager();
        }

        return instance;
    }
}
