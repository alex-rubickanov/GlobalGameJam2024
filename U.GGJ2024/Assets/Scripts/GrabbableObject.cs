using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class GrabbableObject : MonoBehaviour
{
    [SerializeField] protected float throwMultiplier;
    [SerializeField] private float throwHitPowerMultiplier;
    protected Collider col;
    protected Rigidbody rb;

    protected bool wasThrown;

    protected NPlayerGrabbing grabbedByPlayer;
    private NPlayerGrabbing lastGrabbedByPlayer;
    [HideInInspector] public bool isGrabbed => grabbedByPlayer != null;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        if (isGrabbed)
        {
            BeGrabbed();
        }
    }
    protected virtual void BeGrabbed()
    {
        transform.position = grabbedByPlayer.grabPoint.position;
        transform.rotation = grabbedByPlayer.grabPoint.rotation;
    }

    public virtual void Grab(NPlayerGrabbing playerGrabbing)
    {
        lastGrabbedByPlayer = null;
        grabbedByPlayer = playerGrabbing;
        rb.isKinematic = true;
        col.enabled = false;
    }

    public virtual void Throw(Vector3 force)
    {
        wasThrown = true;
        rb.isKinematic = false;
        col.enabled = true;
        rb.AddForce(force * throwMultiplier, ForceMode.Impulse);
        lastGrabbedByPlayer = grabbedByPlayer;
        grabbedByPlayer = null;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            wasThrown = false;
        } else if(other.gameObject.layer == LayerMask.NameToLayer("CharacterBones") && wasThrown)
        {
            NPlayerManager playerManager = other.gameObject.GetComponentInParent<NPlayerManager>();
            if(playerManager.PlayerGrabbing == lastGrabbedByPlayer) return;
            Rigidbody boneRigidbody = other.gameObject.GetComponent<Rigidbody>();
            
            playerManager.RagdollController.EnableRagdoll();
            Vector3 powerDirection = other.GetContact(0).normal;
            boneRigidbody.AddForce(powerDirection + rb.velocity * throwHitPowerMultiplier, ForceMode.Impulse);
            playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
        }
    }
}
