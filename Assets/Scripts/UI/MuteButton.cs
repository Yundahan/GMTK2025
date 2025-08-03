using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    public MuteButton otherButton;

    public string volumeType;

    void Start()
    {
        if (VolumeManager.Instance().GetMute(volumeType))
        {
            GetComponent<Image>().sprite = offSprite;
        } else
        {
            GetComponent<Image>().sprite = onSprite;
        }

        GetComponent<Button>().onClick.AddListener(ToggleMute);
    }

    public void ToggleMute()
    {
        VolumeManager.Instance().ToggleMute(volumeType);
        UpdateSprite();

        if (otherButton != null)
        {
            otherButton.UpdateSprite();
        }
    }

    public void UpdateSprite()
    {
        if (GetComponent<Image>().sprite == onSprite)
        {
            GetComponent<Image>().sprite = offSprite;
        }
        else
        {
            GetComponent<Image>().sprite = onSprite;
        }
    }
}
