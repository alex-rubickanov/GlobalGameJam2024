using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadyConfirmation : MonoBehaviour
{
    private PlayerManager PlayerManager => GetComponent<PlayerManager>();
    
    [SerializeField] private float timeToConfirm;
    private float timer;
    private bool isReady = false;
    
    private bool leftButtonPressed = false;
    private bool rightButtonPressed = false;
    private void Start()
    {
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
                Debug.Log(PlayerManager.PlayerInput.playerIndex + " is ready!");
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
        PlayerManager.SetPlayerReady();
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

    private void OnEnable()
    {
        PlayerManager.InputHandler.OnConfirmLeftStarted += OnConfirmLeftStarted;
        PlayerManager.InputHandler.OnConfirmRightStarted += OnConfirmRightStarted;
        PlayerManager.InputHandler.OnConfirmLeftCanceled += OnConfirmLeftCanceled;
        PlayerManager.InputHandler.OnConfirmRightCanceled += OnConfirmRightCanceled;
    }

    private void OnDisable()
    {
        PlayerManager.InputHandler.OnConfirmLeftStarted -= OnConfirmLeftStarted;
        PlayerManager.InputHandler.OnConfirmRightStarted -= OnConfirmRightStarted;
        PlayerManager.InputHandler.OnConfirmLeftCanceled -= OnConfirmLeftCanceled;
        PlayerManager.InputHandler.OnConfirmRightCanceled -= OnConfirmRightCanceled;
    }
}
