using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    private Vector2 moveInput;

    public Action OnInteractPerformed;
    public Action OnInteractStarted;
    public Action OnInteractCanceled;

    public Action OnJump;
    public Action OnPause;
    public Action OnMeleeHit;

    public Action OnCCLeft;
    public Action OnCCRight;

    public Action OnConfirmLeftStarted;
    public Action OnConfirmRightStarted;
    public Action OnConfirmLeftCanceled;
    public Action OnConfirmRightCanceled;

    public bool WantsToRun { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        ActivateInput();
        SubscribeToInputs(SceneType.Gameplay); //TEMP SOLUTION
    }

    public void SubscribeToInputs(SceneType currentActionMap)
    {
        switch (currentActionMap)
        {
            case SceneType.Gameplay:
                Debug.Log("Subscribed to gameplay");
                playerInput.currentActionMap.FindAction("Move", false).performed += OnMove_performed;
                playerInput.currentActionMap.FindAction("Move", false).canceled += OnMove_canceled;
                playerInput.currentActionMap.FindAction("Interact", false).performed += OnInteract_performed;
                playerInput.currentActionMap.FindAction("Interact", false).started += OnInteract_started;
                playerInput.currentActionMap.FindAction("Interact", false).canceled += OnInteract_canceled;
                playerInput.currentActionMap.FindAction("Jump", false).performed += OnJump_performed;
                playerInput.currentActionMap.FindAction("Run", false).performed += OnRun_performed;
                playerInput.currentActionMap.FindAction("Run", false).canceled += OnRun_canceled;
                playerInput.currentActionMap.FindAction("Pause", false).canceled += OnPause_performed;
                playerInput.currentActionMap.FindAction("MeleeHit", false).performed += OnMeleeHit_performed;
                break;
            case SceneType.ConnectionMenu:
                Debug.Log("Subscribed to connection menu");
                playerInput.currentActionMap.FindAction("ChangeCharacterRight", false).performed +=
                    OnCharacterRight_performed;
                playerInput.currentActionMap.FindAction("ChangeCharacterLeft").performed += OnCharacterLeft_performed;

                playerInput.currentActionMap.FindAction("ConfirmReadyLeft", false).started += OnConfirmLeft_started;
                playerInput.currentActionMap.FindAction("ConfirmReadyRight", false).started += OnConfirmRight_started;
                playerInput.currentActionMap.FindAction("ConfirmReadyLeft", false).canceled += OnConfirmLeft_canceled;
                playerInput.currentActionMap.FindAction("ConfirmReadyRight", false).canceled += OnConfirmRight_canceled;

                break;
            default:
                Debug.LogError($"Player {playerInput.playerIndex} current action map is not found");
                break;
        }
    }

    private void OnConfirmLeft_started(InputAction.CallbackContext obj)
    {
        OnConfirmLeftStarted?.Invoke();
    }

    private void OnConfirmRight_started(InputAction.CallbackContext obj)
    {
        OnConfirmRightStarted?.Invoke();
    }

    private void OnConfirmRight_canceled(InputAction.CallbackContext obj)
    {
        OnConfirmRightCanceled?.Invoke();
    }

    private void OnConfirmLeft_canceled(InputAction.CallbackContext obj)
    {
        OnConfirmLeftCanceled?.Invoke();
    }


    private void OnMeleeHit_performed(InputAction.CallbackContext obj)
    {
        OnMeleeHit?.Invoke();
    }

    private void OnCharacterLeft_performed(InputAction.CallbackContext obj)
    {
        OnCCLeft?.Invoke();
    }

    private void OnCharacterRight_performed(InputAction.CallbackContext obj)
    {
        OnCCRight?.Invoke();
    }

    public void ActivateInputWithDelay(float time)
    {
        StartCoroutine(ActivateInputAfterTime(time));
    }

    private IEnumerator ActivateInputAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ActivateInput();
    }

    private void OnDisable()
    {
        playerInput.DeactivateInput();
    }

    private void OnInteract_started(InputAction.CallbackContext obj)
    {
        OnInteractStarted?.Invoke();
    }

    private void OnInteract_canceled(InputAction.CallbackContext obj)
    {
        OnInteractCanceled?.Invoke();
    }

    private void OnRun_performed(InputAction.CallbackContext obj)
    {
        WantsToRun = true;
    }

    private void OnRun_canceled(InputAction.CallbackContext obj)
    {
        WantsToRun = false;
    }

    private void OnMove_canceled(InputAction.CallbackContext obj)
    {
        moveInput = Vector2.zero;
    }


    private void OnPause_performed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke();
    }

    private void OnJump_performed(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void OnInteract_performed(InputAction.CallbackContext obj)
    {
        OnInteractPerformed?.Invoke();
    }

    private void OnMove_performed(InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<Vector2>();
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    public void ActivateInput()
    {
        playerInput.ActivateInput();
    }

    public void DeactivateInput()
    {
        playerInput.DeactivateInput();
    }
}