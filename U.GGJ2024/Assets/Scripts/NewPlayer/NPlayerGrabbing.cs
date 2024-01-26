using System;
using UnityEngine;

public class NPlayerGrabbing : MonoBehaviour
{
    private NPlayerManager playerManager;
    private Rigidbody rb;
    private Collider col;

    [Header("Throw Power")] [SerializeField]
    private float maxThrowPower;

    [SerializeField] private float minThrowPower;
    [SerializeField] private float throwPowerChargeSpeed;
    [HideInInspector] public bool IsCharging = false;
    private float throwPower;

    [Header("Grabbing")] [SerializeField] 
    private Transform grabPoint;
    [SerializeField] private float delatForInput;
    [HideInInspector] public bool IsAbleToMove;
    private NPlayerGrabbing grabbedPlayer;
    private NPlayerGrabbing grabbedByPlayer;
    private bool isThrowing = false;
    private bool wasThrown;
    [HideInInspector] public bool isGrabbed => grabbedByPlayer != null;
    [HideInInspector] public bool isGrabbing => grabbedPlayer != null;

    [Header("Detection")] 
    [SerializeField] private Transform interactOrigin;
    [SerializeField] private float detectionSphereRadius;


    private void SetupDependencies()
    {
        playerManager = GetComponentInParent<NPlayerManager>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Awake()
    {
        SetupDependencies();
    }

    private void Update()
    {
        if (isGrabbed)
        {
            BeGrabbed();
        }

        if (wasThrown)
        {
            IsAbleToMove = false;
            if (playerManager.PlayerMovement.IsGrounded)
            {
                IsAbleToMove = true;
                playerManager.InputHandler.ActivateInputWithDelay(delatForInput);
                wasThrown = false;
            }
        }

        if (IsCharging)
        {
            ChargePower();
        }
    }

    private void BeGrabbed()
    {
        transform.position = grabbedByPlayer.grabPoint.position;
        transform.rotation = grabbedByPlayer.grabPoint.rotation;
        playerManager.InputHandler.DeactivateInput();
    }

    private void TryGrabPlayer()
    {
        Collider[] results;
        results = Physics.OverlapSphere(interactOrigin.position, detectionSphereRadius);
        foreach (var result in results)
        {
            if (result == GetComponent<Collider>()) continue;
            if (grabbedPlayer) return;
            
            
            if (result.TryGetComponent(out grabbedPlayer))
            {
                GrabPlayer();
            }
        }
    }

    private void GrabPlayer()
    {
        grabbedPlayer.grabbedByPlayer = this;
        grabbedPlayer.col.enabled = false;
        grabbedPlayer.playerManager.RagdollController.EnableRagdoll();
        grabbedPlayer.playerManager.RagdollController.AttachTo(grabPoint);
    }

    private void ThrowPlayer()
    {
        isThrowing = false;
        
        grabbedPlayer.playerManager.RagdollController.UnAttach();
        grabbedPlayer.playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
        grabbedPlayer.playerManager.RagdollController.pelvisRigidbody.AddForce(transform.forward * throwPower * 20, ForceMode.Impulse);
        throwPower = 0;
        grabbedPlayer.wasThrown = true;
        grabbedPlayer.grabbedByPlayer = null;
        grabbedPlayer = null;
    }

    private void OnInteractPerformed()
    {
        if (!isGrabbing && !isGrabbed)
        {
            TryGrabPlayer();
        }
    }


    private void OnInteractStarted()
    {
        if (isGrabbing)
        {
            IsCharging = true;
        }
    }

    private void OnInteractCanceled()
    {
        if (isThrowing)
        {
            IsCharging = false;
            ThrowPlayer();
        }
    }

    private void ChargePower()
    {
        isThrowing = true;
        throwPower += throwPowerChargeSpeed * Time.deltaTime;
        throwPower = Mathf.Clamp(throwPower, minThrowPower, maxThrowPower);
    }

    private void OnEnable()
    {
        playerManager.InputHandler.OnInteractPerformed += OnInteractPerformed;
        playerManager.InputHandler.OnInteractStarted += OnInteractStarted;
        playerManager.InputHandler.OnInteractCanceled += OnInteractCanceled;
    }


    private void OnDisable()
    {
        playerManager.InputHandler.OnInteractPerformed -= OnInteractPerformed;
        playerManager.InputHandler.OnInteractStarted -= OnInteractStarted;
        playerManager.InputHandler.OnInteractCanceled -= OnInteractCanceled;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactOrigin.position, detectionSphereRadius);
    }
}
