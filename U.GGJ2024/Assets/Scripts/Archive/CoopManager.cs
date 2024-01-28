using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]
public class CoopManager : MonoBehaviour
{
    public static CoopManager Instance { get; private set; }
    [HideInInspector] public PlayerInputManager PlayerInputManager;
    [SerializeField] private List<NPlayerManager> players = new List<NPlayerManager>();

    [HideInInspector] public List<Transform> spawnPoints = new List<Transform>();
    [HideInInspector] public SceneType currentSceneType;

    private void Awake()
    {
        PlayerInputManager = GetComponent<PlayerInputManager>();

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
        PlayerInputManager.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.onPlayerLeft += OnPlayerLeft;
    }


    private void OnPlayerLeft(PlayerInput obj)
    {
        NPlayerManager inputHandler = obj.GetComponent<NPlayerManager>();
        players.Remove(inputHandler);
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        NPlayerManager playerManager = obj.GetComponent<NPlayerManager>();

        players.Add(playerManager);


        SetupPlayerActionMap(playerManager.InputHandler);
        SetupPlayerAnimator(playerManager);
        
        PlacePlayerInSpawnPoint(playerManager);

        DontDestroyOnLoad(playerManager.gameObject);
        if (players.Count == 3)
        {
            PlayerInputManager.DisableJoining();
        }
    }

    public List<NPlayerManager> GetPlayers()
    {
        return players;
    }


    private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        SetupAllPlayersActionMap();
        SetupAllPlayersAnimator();
        PlaceAllPlayersInSpawnPoints();
    }
    
    private void PlacePlayerInSpawnPoint(NPlayerManager playerManager)
    {
        playerManager.playerPawn.position = spawnPoints[playerManager.InputHandler.playerInput.playerIndex].position;
        playerManager.playerPawn.rotation = spawnPoints[playerManager.InputHandler.playerInput.playerIndex].rotation;
    }
    
    private void PlaceAllPlayersInSpawnPoints()
    {
        foreach (var player in players)
        {
            PlacePlayerInSpawnPoint(player);
        }
    }

    private void SetupAllPlayersActionMap()
    {
        foreach (var playerManager in players)
        {
            playerManager.InputHandler.playerInput.SwitchCurrentActionMap(currentSceneType.ToString());
            playerManager.InputHandler.SubscribeToInputs(currentSceneType);
        }
    }

    private void SetupAllPlayersAnimator()
    {
        foreach (var playerManager in players)
        {
            SetupPlayerAnimator(playerManager);
        }
    }

    private void SetupPlayerActionMap(NInputHandler inputHandler)
    {
        inputHandler.playerInput.SwitchCurrentActionMap(currentSceneType.ToString());
        inputHandler.SubscribeToInputs(currentSceneType);
    }

    private void SetupPlayerAnimator(NPlayerManager playerManager)
    {
        if(!playerManager.PlayerAnimator) Debug.Log("Player animator is null");
        playerManager.PlayerAnimator.SetAnimator(currentSceneType);
        playerManager.PlayerAnimator.SubscribeToEvents();
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