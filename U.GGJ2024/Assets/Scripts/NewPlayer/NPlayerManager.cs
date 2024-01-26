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
    public NPlayerAnimator PlayerAnimator;
    public RagdollController RagdollController;

    private void Awake()
    {
        InputHandler = GetComponent<NInputHandler>();
        PlayerMovement = GetComponentInChildren<NPlayerMovement>();
        PlayerGrabbing = GetComponentInChildren<NPlayerGrabbing>();
        PlayerAnimator = GetComponentInChildren<NPlayerAnimator>();
        RagdollController = GetComponentInChildren<RagdollController>();
    }
}
