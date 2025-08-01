using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    private PlayerMovement player;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindFirstObjectByType<UIManager>().ToggleMenu();
            Simulation.Instance().ToggleSimulation();
        }

        if (Input.GetKey(KeyCode.R))
        {
            player.GetComponent<PlayerActions>().ActivateJumpBoosting();
        } else if (Input.GetKeyUp(KeyCode.R))
        {
            player.GetComponent<PlayerActions>().DeactivateJumpBoosting();
        }

        if (player != null && Simulation.Instance().IsSimulating())
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            player.SetMove(horizontalAxis);

            if(Mathf.RoundToInt(horizontalAxis) != 0) {
                player.GetComponent<LoopManager>().SetLooping(true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.Jump();
            }
        }
    }

    public bool IsPlayerInteracting()
    {
        return Input.GetKey(KeyCode.E);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindFirstObjectByType<PlayerMovement>();
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
