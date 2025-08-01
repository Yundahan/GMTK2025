using UnityEngine;

public class KeyHint : Hint
{
    public Key key;

    private bool keyPickedUp = false;

    void Update()
    {
        if (hintShown && !keyPickedUp && key.IsInPlayerHand())
        {
            SetText("Press E and aim");
            keyPickedUp = true;
        } else if (keyPickedUp && !key.IsInPlayerHand())
        {
            HideMessage();
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!keyPickedUp)
        {
            hintShown = false;
            HideMessage();
        }
    }
}
