using UnityEngine;
using UnityEngine.UI;

public class LevelSelectImage : MonoBehaviour
{
    public bool firstLevelScreen = false;

    private const int SCROLL_DISTANCE = 50;
    private const float SCROLL_SPEED = 500f;

    private float minY;
    private float maxY;

    void Awake()
    {
        minY = Screen.height - GetComponent<RectTransform>().rect.height / 2 + SCROLL_DISTANCE;
        maxY = GetComponent<RectTransform>().rect.height / 2 - SCROLL_DISTANCE;

        foreach (LevelButton levelButton in GetComponentsInChildren<LevelButton>())
        {
            if (levelButton.sceneName == SceneLoader.Instance().GetActiveSceneName())
            {
                // Center the button leading to the current scene
                float targetY = Mathf.Clamp(Screen.height / 2 - levelButton.transform.localPosition.y, minY, maxY);
                this.transform.position = new Vector3(this.transform.position.x, targetY, 0);
            }
        }
    }

    void Update()
    {
        float mouseY = Input.mousePosition.y;
        float currentY = this.transform.position.y;

        if (mouseY < SCROLL_DISTANCE && currentY < maxY)
        {
            this.transform.position += new Vector3(0, SCROLL_SPEED * Time.unscaledDeltaTime, 0);
        } else if (mouseY > Screen.height - SCROLL_DISTANCE && currentY > minY)
        {
            this.transform.position -= new Vector3(0, SCROLL_SPEED * Time.unscaledDeltaTime, 0);
        }
    }
}
