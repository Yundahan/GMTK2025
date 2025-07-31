using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShadowManager : MonoBehaviour
{
    private static ShadowManager instance;

    public GameObject playerShadow;

    private PlayerBehaviour player;

    private const float LOOP_TIME = 5f;
    private const float PATH_INTERVAL = 0.01f; // time in seconds between points on the recorded path of a shadow

    private List<ShadowMovement> shadows = new();
    private Vector2 spawnPoint;
    private List<Vector2> playerPath = new();
    private bool looping = false;
    private float loopStartTime = 0f;
    private float lastPositionTime = 0f;
    private int pathPosition = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            player = FindFirstObjectByType<PlayerBehaviour>();

            if (player != null)
            {
                spawnPoint = player.transform.position;
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (looping)
        {
            if (Time.time - lastPositionTime > PATH_INTERVAL)
            {
                playerPath.Add(player.gameObject.transform.position);

                foreach (ShadowMovement shadowMovement in shadows)
                {
                    shadowMovement.ContinueOnPath(pathPosition);
                }

                lastPositionTime = Time.time;
                pathPosition++;
            }

            if (Time.time - loopStartTime > LOOP_TIME)
            {
                EndLoop();
            }
        }
    }

    private void EndLoop()
    {
        player.Reset();

        foreach (ShadowMovement oldShadow in shadows)
        {
            oldShadow.Reset();
        }

        GameObject newShadow = Instantiate(this.playerShadow, this.spawnPoint, Quaternion.identity);
        ShadowMovement newShadowMovement = newShadow.GetComponent<ShadowMovement>();
        newShadowMovement.SetSpawnPoint(spawnPoint);
        newShadowMovement.SetPath(playerPath);
        shadows.Add(newShadowMovement);
        playerPath = new();
        pathPosition = 0;
        loopStartTime = Time.time;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetLooping(false);
    }

    public void SetLooping(bool value)
    {
        // if looping is set to true and it wasnt before, set the times correctly
        if (value != looping && value)
        {
            playerPath = new();
            shadows.Clear();
            lastPositionTime = Time.time;
            loopStartTime = Time.time;
        }

        looping = value;
    }

    public void Reset()
    {
        playerPath.Clear();
        shadows.Clear();
    }

    public bool GetLooping()
    {
        return looping;
    }

    public float GetPathInterval()
    {
        return PATH_INTERVAL;
    }

    public static ShadowManager Instance()
    {
        if (instance == null)
        {
            instance = new ShadowManager();
        }

        return instance;
    }
}
