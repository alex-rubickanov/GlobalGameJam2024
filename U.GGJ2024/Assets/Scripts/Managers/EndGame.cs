using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameTimerText;
    [SerializeField] private float gameTimeInSeconds = 300.0f;

    [SerializeField] private GameObject player1win;
    [SerializeField] private GameObject player2win;
    [SerializeField] private GameObject player3win;
    
    private PointsGainUIManager pointsGainUiManager;
    private bool timerStop = false;
    private void Update()
    {
        if (!timerStop)
        {
            gameTimeInSeconds -= Time.deltaTime;
            int munites = Mathf.FloorToInt(gameTimeInSeconds / 60f);
            int second = Mathf.FloorToInt(gameTimeInSeconds % 60f);
            gameTimerText.text = munites.ToString("00") + ":" + second.ToString("00");
        }
        

        if (gameTimeInSeconds <= 0.0f)
        {
            FinishGame();
            timerStop = true;
        }
    }

    private void FinishGame()
    {
        int winnerIndex = pointsGainUiManager.GetTheWinnerIndex();

        switch (winnerIndex)
        {
            case 0:
                Debug.Log("Player 1 wins");
                player1win.SetActive(true);
                break;
            case 1:
                Debug.Log("Player 2 wins");
                player2win.SetActive(true);
                break;
            case 2:
                Debug.Log("Player 3 wins");
                player3win.SetActive(true);
                break;
            default:
                Debug.Log("No one wins");
                break;
        }

        Time.timeScale = 0.3f;
    }
}
