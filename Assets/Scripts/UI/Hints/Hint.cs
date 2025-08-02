using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public TextMeshProUGUI text;
    protected bool hintShown = false;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            ShowMessage();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        hintShown = false;
        HideMessage();
    }

    protected void ShowMessage()
    {
        if (!hintShown)
        {
            hintShown = true;
            this.text.gameObject.SetActive(true);
        }
    }

    protected void HideMessage()
    {
        this.text.gameObject.SetActive(false);
    }

    protected void SetText(string text)
    {
        this.text.text = text;
    }
}
