using UnityEngine;
using UnityEngine.UI;

public class LevelSelectImage : MonoBehaviour
{
    public bool firstLevelScreen = false;

    private const int SCROLL_DISTANCE = 50;
    private const float SCROLL_SPEED = 3f;

    private float minY;
    private float maxY;

    void Awake()
    {
        minY = Screen.height - GetComponent<RectTransform>().rect.height / 2 + SCROLL_DISTANCE;
        maxY = GetComponent<RectTransform>().rect.height / 2 - SCROLL_DISTANCE;
    }

    void Update()
    {
        float mouseY = Input.mousePosition.y;
        float currentY = this.transform.position.y;

        if (mouseY < SCROLL_DISTANCE && currentY < maxY)
        {
            this.transform.position += new Vector3(0, SCROLL_SPEED * Time.time, 0);
        } else if (mouseY > Screen.height - SCROLL_DISTANCE && currentY > minY)
        {
            this.transform.position -= new Vector3(0, SCROLL_SPEED * Time.time, 0);
        }
    }
}
