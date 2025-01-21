using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(NextScene);
    }

    private void NextScene()
    {
        SceneLoader.Instance().LoadScene(1 - SceneLoader.Instance().GetActiveSceneBuildIndex());
    }
}
