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
        canBeGrabbed = false;
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
            }
        }
    }

    public override void Grab(NPlayerGrabbing playerGrabbing)
    {
        if(!canBeGrabbed) return;
        
        base.Grab(playerGrabbing);
        
        playerManager.InputHandler.DeactivateInput();
        playerManager.RagdollController.EnableRagdoll();
        FixedJoint joint = playerGrabbing.grabPoint.GetComponent<FixedJoint>();
        playerManager.RagdollController.AttachPelvisTo(joint);
    }

    public override void Throw(Vector3 force)
    {
        FixedJoint joint = grabbedByPlayer.grabPoint.GetComponent<FixedJoint>();
        playerManager.RagdollController.UnAttach(joint);
        playerManager.RagdollController.pelvisRigidbody.AddForce(force * throwMultiplier, ForceMode.Impulse);
        playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
        wasThrown = true;
        grabbedByPlayer = null;
    }
}
