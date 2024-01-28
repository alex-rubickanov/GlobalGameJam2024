using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerAnimator : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController connectionMenuController;
    [SerializeField] private RuntimeAnimatorController gameplayController;
    [SerializeField] private RuntimeAnimatorController currentAnimatorController;

    private NPlayerManager playerManager;
    private Animator animator;

    private static readonly int IsGrabbing = Animator.StringToHash("IsGrabbing");
    private static readonly int Velocity = Animator.StringToHash("Velocity");
    private static readonly int IsCharging = Animator.StringToHash("IsCharging");
    private static readonly int IsGrabbed = Animator.StringToHash("IsGrabbed");
    private static readonly int IsAbleToMove = Animator.StringToHash("IsAbleToMove");
    private static readonly int MeleeHit = Animator.StringToHash("MeleeHit");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Grounded = Animator.StringToHash("Grounded");

    private static readonly int Cheer1 = Animator.StringToHash("Cheer1");
    private static readonly int Cheer2 = Animator.StringToHash("Cheer2");
    private static readonly int Cheer3 = Animator.StringToHash("Cheer3");
    private static readonly int Wave = Animator.StringToHash("Wave");
    private static readonly int Default = Animator.StringToHash("Default");

    public Action OnMeleeHit;
    private PlayerCharacterSelector playerCharacterSelector;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCharacterSelector = GetComponentInParent<PlayerCharacterSelector>();
        
    }

    private void Start()
    {
        playerManager = GetComponentInParent<NPlayerManager>();
        
    }

    public void SubscribeToEvents()
    {
        if (currentAnimatorController == gameplayController)
        {
            Debug.Log("Animator subscribed to gameplay");
            playerManager.PlayerMovement.OnJumpStart += OnJumpStart;
        }
        else if (currentAnimatorController == connectionMenuController)
        {
            Debug.Log("Animator subscribed to connection menu");
            playerCharacterSelector.OnChangeCharacter += OnChangeCharacter;
        }
    }

    private void OnJumpStart()
    {
        animator.SetTrigger(Jump);
    }

    private void OnChangeCharacter()
    {
        //animator.SetTrigger(Default);
        PlayRandomCheer();
    }

    private void PlayRandomCheer()
    {
        int randomCheer = UnityEngine.Random.Range(0, 4);
        switch (randomCheer)
        {
            case 0:
                animator.SetTrigger(Cheer1);
                break;
            case 1:
                animator.SetTrigger(Cheer2);
                break;
            case 2:
                animator.SetTrigger(Cheer3);
                break;
            case 3:
                animator.SetTrigger(Wave);
                break;
            default:
                Debug.LogError($"Error: {randomCheer} is not a valid random cheer");
                break;
        }
    }

    private void OnMeleeHitStart()
    {
        animator.SetTrigger(MeleeHit);
    }

    private void Update()
    {
        if (currentAnimatorController == gameplayController)
        {
            animator.SetFloat(Velocity, playerManager.PlayerMovement.GetVelocityMagnitude());
            animator.SetBool(Grounded, playerManager.PlayerMovement.IsGrounded);

            animator.SetBool(IsGrabbing, playerManager.PlayerGrabbing.isGrabbing);
            animator.SetBool(IsCharging, playerManager.PlayerGrabbing.IsCharging);
            animator.SetBool(IsGrabbed, playerManager.GrabbablePlayer.isGrabbed);
            animator.SetBool(IsAbleToMove, playerManager.PlayerGrabbing.IsAbleToMove);
        }
        else if (currentAnimatorController == connectionMenuController)
        {
            animator.SetFloat(Velocity, 0);
        }
    }

    public void MeleeHitAnimationEvent()
    {
        OnMeleeHit?.Invoke();
    }

    private void SetConnectionMenuController()
    {
        animator.runtimeAnimatorController = connectionMenuController;
        currentAnimatorController = connectionMenuController;
    }

    private void SetGameplayController()
    {
        animator.runtimeAnimatorController = gameplayController;
        currentAnimatorController = gameplayController;
    }

    public void SetAnimator(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.ConnectionMenu:
                SetConnectionMenuController();
                SubscribeToEvents();
                break;
            case SceneType.Gameplay:
                SetGameplayController();
                SubscribeToEvents();
                break;
            default:
                Debug.LogError($"Error: {sceneType} is not a valid SceneAnimatorEnum");
                break;
        }
    }
}