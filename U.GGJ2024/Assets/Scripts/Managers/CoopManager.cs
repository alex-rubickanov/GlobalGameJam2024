using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerInputManager))]
public class CoopManager : MonoBehaviour
{
    public static CoopManager Instance { get; private set; }
    [HideInInspector] public PlayerInputManager PlayerInputManager;
    [SerializeField] private List<PlayerManager> players = new List<PlayerManager>();

    [HideInInspector] public List<Transform> spawnPoints = new List<Transform>();
    [HideInInspector] public SceneType currentSceneType;

    private void Awake()
    {
        PlayerInputManager = GetComponent<PlayerInputManager>();
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerInputManager.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.onPlayerLeft += OnPlayerLeft;
    }


    private void OnPlayerLeft(PlayerInput obj)
    {
        PlayerManager inputHandler = obj.GetComponent<PlayerManager>();
        players.Remove(inputHandler);
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        PlayerManager playerManager = obj.GetComponent<PlayerManager>();

        players.Add(playerManager);

        SetupPlayerSpawnPoint(playerManager);

        SetupPlayerActionMap(playerManager);
        SetupPlayerAnimator(playerManager);

        DontDestroyOnLoad(playerManager.gameObject);
        if (players.Count == 3)
        {
            PlayerInputManager.DisableJoining();
        }
    }

    public List<PlayerManager> GetPlayers()
    {
        return players;
    }


    private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        SetupAllPlayersActionMap();
        SetupAllPlayersAnimator();
        SetupAllPlayersSpawnPoints();
    }
    
    private void SetupPlayerSpawnPoint(PlayerManager playerManager)
    {
        int playerIndex = players.IndexOf(playerManager);
        playerManager.SetSpawnPoint(spawnPoints[playerIndex]);
    }

    private void SetupAllPlayersSpawnPoints()
    {
        foreach (var playerManager in players)
        {
            int playerIndex = players.IndexOf(playerManager);
            playerManager.SetSpawnPoint(spawnPoints[playerIndex]);
        }
    }

    private void SetupAllPlayersActionMap()
    {
        foreach (var playerManager in players)
        {
            playerManager.PlayerInput.SwitchCurrentActionMap(currentSceneType.ToString());
            playerManager.InputHandler.SubscribeToInputs(currentSceneType);
        }
    }

    private void SetupAllPlayersAnimator()
    {
        foreach (var playerManager in players)
        {
            playerManager.PlayerAnimator.SetAnimator(currentSceneType);
            playerManager.PlayerAnimator.Subscribe();
        }
    }

    private void SetupPlayerActionMap(PlayerManager playerManager)
    {
        playerManager.PlayerInput.SwitchCurrentActionMap(currentSceneType.ToString());
        playerManager.InputHandler.SubscribeToInputs(currentSceneType);
    }

    private void SetupPlayerAnimator(PlayerManager playerManager)
    {
        playerManager.PlayerAnimator.SetAnimator(currentSceneType);
        playerManager.PlayerAnimator.Subscribe();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
    }
}