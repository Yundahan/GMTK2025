using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ResetScene);
    }

    private void ResetScene()
    {
        SFXManager.Instance().PlaySFX("Shadow");
        SceneLoader.Instance().ReloadScene();
    }
}
