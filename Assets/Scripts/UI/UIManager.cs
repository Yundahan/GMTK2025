using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject uiMessage;
    public GameObject gameUI;
    public GameObject menuUI;
    public GameObject zeigerParent;
    public List<Image> shadowIndicators;
    public Sprite shadowInReserve;
    public Sprite shadowInPlay;

    private Dictionary<string, GameObject> currentTextsInGameGUI = new Dictionary<string, GameObject>();
    private LoopManager loopManager;
    private float zeigerAngle = 0f;
    private float angularSpeed;

    void Awake()
    {
        loopManager = FindFirstObjectByType<LoopManager>();
        angularSpeed = 360f / loopManager.GetLoopDuration();

        for (int i = shadowIndicators.Count - 1; i >= 0; i--)
        {
            if (i >= loopManager.GetMaxShadows())
            {
                shadowIndicators[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (loopManager.GetLooping())
        {
            zeigerAngle -= angularSpeed * Time.deltaTime;
            zeigerParent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zeigerAngle));
        }
    }

    public void ToggleMenu()
    {
        Simulation.Instance().ToggleSimulation();
        gameUI.SetActive(!gameUI.activeSelf);
        menuUI.SetActive(!menuUI.activeSelf);
    }

    /// <summary>
    /// Shows a text message at a given position on the game UI
    /// </summary>
    /// <param name="anchor">The position of the anchor, coordinate values between 0 (bottom/left) and 1 (top/right).</param>
    /// <param name="position">The position of the text relative to the anchor.</param>
    /// <param name="text">String containing the text to display.</param>
    public string ShowTextInGameGUI(Vector2 anchor, Vector2 position, string text)
    {
        GameObject uiMessage = Instantiate(this.uiMessage);
        uiMessage.GetComponent<RectTransform>().SetParent(gameUI.transform);
        uiMessage.GetComponent<RectTransform>().anchorMin = anchor;
        uiMessage.GetComponent<RectTransform>().anchorMax = anchor;
        uiMessage.GetComponent<RectTransform>().anchoredPosition = position;
        uiMessage.GetComponentInChildren<TextMeshProUGUI>().text = text;
        string guid = Guid.NewGuid().ToString();
        currentTextsInGameGUI[guid] = uiMessage;
        return guid;
    }

    /// <summary>
    /// Removes a UI message from the game UI by ID.
    /// </summary>
    /// <param name="guid">The GUID of the text.</param>
    public void RemoveTextInGameGUI(string guid)
    {
        if (guid == null || !currentTextsInGameGUI.ContainsKey(guid))
        {
            return;
        }

        GameObject uiMessage = currentTextsInGameGUI[guid];
        currentTextsInGameGUI.Remove(guid);
        Destroy(uiMessage);
    }

    /// <summary>
    /// Removes all UI messages from the game UI.
    /// </summary>
    public void RemoveAllTextsInGameGUI()
    {
        foreach (KeyValuePair<string, GameObject> entry in currentTextsInGameGUI)
        {
            Destroy(entry.Value);
        }

        currentTextsInGameGUI.Clear();
    }

    public void ShowShadowIndicators(int shadowNumber)
    {
        for (int i = shadowIndicators.Count - 1; i >= 0; i--)
        {
            if (i < shadowNumber)
            {
                shadowIndicators[i].sprite = shadowInReserve;
            } else
            {
                shadowIndicators[i].sprite = shadowInPlay;
            }
        }
    }

    public void PlayButtonSFX()
    {
        SFXManager.Instance().PlaySFX("Button");
    }

    public void PlayLevelSelectSFX()
    {
        SFXManager.Instance().PlaySFX("Level_Select");
    }

    public void Reset()
    {
        zeigerAngle = 0f;
        zeigerParent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
