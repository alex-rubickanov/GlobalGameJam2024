using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandlers : MonoBehaviour
{
    [SerializeField] public SceneType sceneType;
    [SerializeField] public bool enableJoining;
    [SerializeField] public Transform[] spawnPoints;

    public static SceneHandlers Instance { get; private set; }

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

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainScene()
    {
        foreach (var player in NCoopManager.Instance.players)
        {
            Destroy(player.gameObject);
        }

        SceneManager.LoadScene(0);
    }
}