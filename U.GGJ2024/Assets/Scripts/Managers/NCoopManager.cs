using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]
public class NCoopManager : MonoBehaviour
{
    public static NCoopManager Instance { get; private set; }
    public PlayerInputManager playerInputManager;
    
    public List<NPlayerManager> players = new List<NPlayerManager>();
    [HideInInspector] public Transform[] spawnPoints;
    [HideInInspector] public SceneType currentSceneType;
    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        SceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        currentSceneType = SceneHandler.Instance.sceneType;
        spawnPoints = SceneHandler.Instance.spawnPoints;
        if (SceneHandler.Instance.enableJoining)
        {
            playerInputManager.EnableJoining();
        }
        else
        {
            playerInputManager.DisableJoining();
        }
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        NPlayerManager playerManager = obj.GetComponent<NPlayerManager>();
        players.Add(playerManager);

        playerManager.SetSpawnPoint(spawnPoints[players.Count - 1]);
        
        DontDestroyOnLoad(playerManager.gameObject);
    }


    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }
    
}
