using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class NPlayerManager : MonoBehaviour
{
    public NInputHandler InputHandler;
    public NPlayerMovement PlayerMovement;
    public NPlayerGrabbing PlayerGrabbing;
    public GrabbablePlayer GrabbablePlayer;
    public NPlayerAnimator PlayerAnimator;
    public RagdollController RagdollController;
    public MeleeCombat PlayerCombat;

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
    
    public bool CanAttack()
    {
        return !GrabbablePlayer.isGrabbed && !PlayerGrabbing.isGrabbing;
    }
}
