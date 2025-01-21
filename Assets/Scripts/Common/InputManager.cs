using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    private PlayerBehaviour player;

    string guid = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(Simulation.Instance().IncreaseECount());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindFirstObjectByType<UIManager>().ToggleMenu();
            Simulation.Instance().ToggleSimulation();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            guid = FindFirstObjectByType<UIManager>().ShowTextInGameGUI(new Vector2(0f, 1f), new Vector2(250f, -120f), "hier ist ein text");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            FindFirstObjectByType<UIManager>().RemoveTextInGameGUI(guid);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            FindFirstObjectByType<UIManager>().RemoveAllTextsInGameGUI();
        }

        if (player != null)
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");

            player.Move(Mathf.RoundToInt(horizontalAxis), Mathf.RoundToInt(verticalAxis));
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindFirstObjectByType<PlayerBehaviour>();
    }

    public static InputManager Instance()
    {
        if (instance == null)
        {
            instance = new InputManager();
        }

        return instance;
    }
}
