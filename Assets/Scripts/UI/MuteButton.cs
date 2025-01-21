using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public string volumeType;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToggleMute);
    }

    public void ToggleMute()
    {
        VolumeManager.Instance().ToggleMute(volumeType);
    }
}
