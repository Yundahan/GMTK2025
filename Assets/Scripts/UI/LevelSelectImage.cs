using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectImage : MonoBehaviour
{
    public bool startScreen = false;

    private const int SCROLL_DISTANCE = 50;
    private const float SCROLL_SPEED = 500f;

    private float minY;
    private float maxY;
    private float scaledScrollSpeed;

    void Awake()
    {
        float uiSizeFactor = Screen.height / 1080f;
        float height = GetComponent<RectTransform>().rect.height;
        minY = Screen.height - uiSizeFactor * height / 2;
        maxY = uiSizeFactor * height / 2;
        scaledScrollSpeed = SCROLL_SPEED * uiSizeFactor;

        foreach (LevelButton levelButton in GetComponentsInChildren<LevelButton>())
        {
            if (!startScreen && levelButton.sceneName == SceneLoader.Instance().GetActiveSceneName())
            {
                // Center the button leading to the current scene
                float targetY = Mathf.Clamp(Screen.height / 2 - levelButton.transform.localPosition.y, minY, maxY);
                this.transform.position = new Vector3(this.transform.position.x, targetY, 0);
            }
        }
    }

    void Update()
    {
        float elapsedTime = Time.unscaledDeltaTime;

        // This can occur during first loading
        if (elapsedTime > 0.5f)
        {
            return;
        }

        float mouseY = Input.mousePosition.y;
        float currentY = this.transform.position.y;

        if (mouseY < SCROLL_DISTANCE && currentY < maxY)
        {
            this.transform.position += new Vector3(0, scaledScrollSpeed * elapsedTime, 0);
        } else if (mouseY > Screen.height - SCROLL_DISTANCE && currentY > minY)
        {
            this.transform.position -= new Vector3(0, scaledScrollSpeed * elapsedTime, 0);
        }
    }
}
