using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Slider>().value = VolumeManager.Instance().GetMasterVolume();
    }

    public void SetVolume(float sliderValue)
    {
        VolumeManager.Instance().SetMasterVolume(sliderValue);
    }
}
