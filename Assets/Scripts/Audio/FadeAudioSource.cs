using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class FadeAudioSource
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    public static IEnumerator EndFade(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float start = 1f-audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = 1f-(Mathf.Lerp(start, 1f, currentTime / duration));
            if (audioSource.volume == 0f)
                {
                audioSource.mute = true;
                }
            yield return null;
        }
        yield break;
    }

}