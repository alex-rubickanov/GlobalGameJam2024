using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private SceneType sceneType;
    [SerializeField] private bool enableJoining;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    public static SceneHandler Instance { get; private set; }

    private void Awake()
    {
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
        PassDataToCoopManager();
    }

    private void PassDataToCoopManager()
    {
        if (enableJoining)
        {
            CoopManager.Instance.PlayerInputManager.EnableJoining();
        }
        else
        {
            CoopManager.Instance.PlayerInputManager.DisableJoining();
        }
        
        CoopManager.Instance.currentSceneType = sceneType;
        
        CoopManager.Instance.spawnPoints = spawnPoints;
    }
}