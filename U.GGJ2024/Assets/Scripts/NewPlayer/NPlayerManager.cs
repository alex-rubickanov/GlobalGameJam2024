using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class NPlayerManager : MonoBehaviour
{
    [HideInInspector] public NInputHandler InputHandler;
    [HideInInspector] public NPlayerMovement PlayerMovement;
    [HideInInspector] public NPlayerGrabbing PlayerGrabbing;
    [HideInInspector] public GrabbablePlayer GrabbablePlayer;
    [HideInInspector] public NPlayerAnimator PlayerAnimator;
    [HideInInspector] public RagdollController RagdollController;
    [HideInInspector] public MeleeCombat PlayerCombat;

    public Transform playerPawn;
    public Transform playerModel;
    [SerializeField] private int pressToUnStun = 3;
    
    public bool isStun = false;
    private int pressCount = 0;
    
    private StunObject stunObject;

    private void Awake()
    {
        InputHandler = GetComponent<NInputHandler>();
        PlayerMovement = GetComponentInChildren<NPlayerMovement>();
        PlayerGrabbing = GetComponentInChildren<NPlayerGrabbing>();
        PlayerAnimator = GetComponentInChildren<NPlayerAnimator>();
        RagdollController = GetComponentInChildren<RagdollController>();
        GrabbablePlayer = GetComponentInChildren<GrabbablePlayer>();
        PlayerCombat = GetComponentInChildren<MeleeCombat>();
    }

    private void Start()
    {
        InputHandler.OnUnStun += UnStan_pressed;
    }

    private void UnStan_pressed()
    {
        if (!isStun) return;
        
        pressCount++;
        if (pressCount >= pressToUnStun)
        {
            UnStan(stunObject.playerPawnHolder.forward * 10f);
            pressCount = 0;
        }
    }

    public void Stun(StunObject _stunObject)
    {
        stunObject = _stunObject;
        isStun = true;
        
        RagdollController.DisableWithoutAnimation();
        
        RagdollController.playerCollider.enabled = false;
        RagdollController.playerRigidbody.isKinematic = true;
        
        PlayerMovement.canMove = false;
    }

    public void UnStan(Vector3 force)
    {
        isStun = false;
        stunObject.UnTrapPlayer();
        RagdollController.EnableRagdoll();
        RagdollController.pelvisRigidbody.AddForce(force, ForceMode.Impulse);
        RagdollController.DisableRagdollWithDelay(3f);
    }
}
