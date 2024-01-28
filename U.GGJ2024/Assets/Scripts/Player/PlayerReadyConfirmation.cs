using UnityEngine;

public class PlayerReadyConfirmation : MonoBehaviour
{
    private NPlayerManager playerManager;
    
    [SerializeField] private float timeToConfirm;
    private float timer;
    private bool isReady = false;
    
    private bool leftButtonPressed = false;
    private bool rightButtonPressed = false;
    private void Start()
    {
        playerManager = GetComponent<NPlayerManager>();
        
        ResetTimer();
    }

    private void Update()
    {
        BeginConfirmation();
    }

    private void BeginConfirmation()
    {
        if (leftButtonPressed && rightButtonPressed && !isReady)
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);
            if (timer <= 0)
            {
                Confirm();
                Debug.Log(playerManager.InputHandler.playerInput.playerIndex + " is ready!");
            }
        }
        else
        {
            ResetTimer();
        }
        
    }

    private void Confirm()
    {
        isReady = true;
        //playerManager.SetPlayerReady();
    }

    private void ResetTimer()
    {
        timer = timeToConfirm;
    }
    private void OnConfirmRightCanceled()
    {
        rightButtonPressed = false;
    }

    private void OnConfirmLeftCanceled()
    {
        leftButtonPressed = false;
    }

    private void OnConfirmRightStarted()
    {
        rightButtonPressed = true;
    }

    private void OnConfirmLeftStarted()
    {
        leftButtonPressed = true;
    }
}
