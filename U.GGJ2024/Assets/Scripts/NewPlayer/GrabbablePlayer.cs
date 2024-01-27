using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbablePlayer : GrabbableObject
{
    public NPlayerManager playerManager;
    
    [SerializeField] private float delatForInput;
    [HideInInspector] public bool IsAbleToMove;

    protected override void Start()
    {
        base.Start();
        
        playerManager = GetComponentInParent<NPlayerManager>();
    }

    protected override void Update()
    {
        if (wasThrown)
        {
            IsAbleToMove = false;
            if (playerManager.PlayerMovement.IsGrounded)
            {
                IsAbleToMove = true;
                wasThrown = false;
            }
        }
    }

    public override void Grab(NPlayerGrabbing playerGrabbing)
    {
        base.Grab(playerGrabbing);
        
        playerManager.InputHandler.DeactivateInput();
        playerManager.RagdollController.EnableRagdoll();
        playerManager.RagdollController.AttachTo(playerGrabbing.grabPoint);
    }

    public override void Throw(Vector3 force)
    {
        playerManager.RagdollController.UnAttach();
        playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
        playerManager.RagdollController.pelvisRigidbody.AddForce(force * throwMultiplier, ForceMode.Impulse);
        wasThrown = true;
        grabbedByPlayer = null;
    }
}
