using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController connectionMenuController;
    [SerializeField] private RuntimeAnimatorController gameplayController;
    [SerializeField] private RuntimeAnimatorController currentAnimatorController;
    private Animator animator => GetComponent<Animator>();
    
    private PlayerManager PlayerManager => GetComponentInParent<PlayerManager>();
    
    private static readonly int IsGrabbing = Animator.StringToHash("IsGrabbing");
    private static readonly int Velocity = Animator.StringToHash("Velocity");
    private static readonly int IsCharging = Animator.StringToHash("IsCharging");
    private static readonly int IsGrabbed = Animator.StringToHash("IsGrabbed");
    private static readonly int IsAbleToMove = Animator.StringToHash("IsAbleToMove");
    private static readonly int MeleeHit = Animator.StringToHash("MeleeHit");

    public Action OnMeleeHit;
    private static readonly int Cheer1 = Animator.StringToHash("Cheer1");
    private static readonly int Cheer2 = Animator.StringToHash("Cheer2");
    private static readonly int Cheer3 = Animator.StringToHash("Cheer3");
    private static readonly int Wave = Animator.StringToHash("Wave");
    private static readonly int Default = Animator.StringToHash("Default");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private PlayerCharacterSelector playerCharacterSelector => GetComponentInParent<PlayerCharacterSelector>();

    public void Subscribe()
    {
        if (currentAnimatorController == gameplayController)
        {
            Debug.Log("Animator subscribed to gameplay");
            PlayerManager.PlayerMelee.OnMeleeHitStart += OnMeleeHitStart;
            OnMeleeHit += PlayerManager.PlayerMelee.DetectHit;
            PlayerManager.PlayerMovement.OnJumpStart += OnJumpStart;
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
            animator.SetFloat(Velocity, PlayerManager.PlayerMovement.GetVelocityMagnitude());
            animator.SetBool(Grounded, PlayerManager.PlayerMovement.IsGrounded);
            
            animator.SetBool(IsGrabbing, PlayerManager.PlayerGrabbing.isGrabbing);
            animator.SetBool(IsCharging, PlayerManager.PlayerGrabbing.IsCharging);
            animator.SetBool(IsGrabbed, PlayerManager.PlayerGrabbing.isGrabbed);
            animator.SetBool(IsAbleToMove, PlayerManager.PlayerGrabbing.IsAbleToMove);
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
                break;
            case SceneType.Gameplay:
                SetGameplayController();
                break;
            default:
                Debug.LogError($"Error: {sceneType} is not a valid SceneAnimatorEnum");
                break;
        }
    }
}