using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public NPlayerManager playerManager;

    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform pelvis;
    public Rigidbody pelvisRigidbody;
    private Animator animator;
    public Collider playerCollider;
    public Rigidbody playerRigidbody;

    public Rigidbody[] ragdollBones;
    public List<Rigidbody> ragdollBonesList;
    [HideInInspector] public Collider[] ragdollColliders;
    
    public Rigidbody rightLegRigidbody;
    public Rigidbody leftLegRigidbody;
    public Rigidbody headRigidbody;

    public bool isRagdoll = false;

    private Coroutine stopRagdollCoroutine = null;

    private void Start()
    {
        playerManager = GetComponentInParent<NPlayerManager>();
        animator = GetComponentInChildren<Animator>();

        ragdollBones = rootBone.GetComponentsInChildren<Rigidbody>();
        ragdollColliders = rootBone.GetComponentsInChildren<Collider>();

        pelvisRigidbody = pelvis.GetComponent<Rigidbody>();

        playerCollider = GetComponent<Collider>();
        playerRigidbody = GetComponent<Rigidbody>();
        
        ragdollBonesList = ragdollBones.ToList();
        InitRagdoll();
    }

    public void DisableRagdoll()
    {
        isRagdoll = false;

        playerManager.GrabbablePlayer.canBeGrabbed = false;

        playerRigidbody.isKinematic = false;

        foreach (var ragdoll in ragdollBones)
        {
            ragdoll.isKinematic = true;
        }

        playerCollider.enabled = true;


        AlignRotationToHips();
        AlignPositionToHips();

        playerManager.playerPawn.rotation = new Quaternion(0, playerManager.playerPawn.rotation.y,
            0, playerManager.playerPawn.rotation.w);

        animator.enabled = true;
        PlayGetUpAnimation();

        playerManager.GrabbablePlayer.wasThrown = false;

        playerManager.PlayerMovement.canMove = true;
        playerManager.InputHandler.ActivateInput();
    }

    public void DisableWithoutAnimation()
    {
        isRagdoll = false;

        playerManager.GrabbablePlayer.canBeGrabbed = false;

        playerRigidbody.isKinematic = false;

        foreach (var ragdoll in ragdollBones)
        {
            ragdoll.isKinematic = true;
        }

        playerCollider.enabled = true;


        AlignRotationToHips();
        AlignPositionToHips();

        playerManager.playerPawn.rotation = new Quaternion(0, playerManager.playerPawn.rotation.y,
            0, playerManager.playerPawn.rotation.w);

        animator.enabled = true;

        playerManager.GrabbablePlayer.wasThrown = false;

        playerManager.PlayerMovement.canMove = true;
        playerManager.InputHandler.ActivateInput();
    }

    public void EnableRagdoll()
    {
        Reset();
        if (stopRagdollCoroutine != null)
        {
            StopCoroutine(stopRagdollCoroutine);
        }

        if (playerManager.PlayerGrabbing.isGrabbing)
        {
            playerManager.PlayerGrabbing.LooseObject();
        }

        isRagdoll = true;

        playerCollider.enabled = false;

        playerManager.GrabbablePlayer.canBeGrabbed = true;

        playerRigidbody.isKinematic = true;

        foreach (var ragdoll in ragdollBones)
        {
            ragdoll.isKinematic = false;
        }

        animator.enabled = false;


        playerManager.InputHandler.DeactivateInput();
    }

    void InitRagdoll()
    {
        foreach (var ragdoll in ragdollBones)
        {
            ragdoll.isKinematic = true;
            ragdoll.transform.AddComponent<BoneRagdollTrigger>();
        }
    }

    private void AlignPositionToHips()
    {
        Vector3 originalPelvisPosition = pelvis.position;
        transform.position = pelvis.position;


        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo);
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }

        pelvis.position = originalPelvisPosition;
    }

    private void AlignRotationToHips()
    {
        Vector3 originalPelvisPosition = pelvis.position;
        Quaternion originalPelvisRotation = pelvis.rotation;

        Vector3 desiredDirection = pelvis.up * -1;
        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
        transform.rotation *= fromToRotation;

        pelvis.position = originalPelvisPosition;
        pelvis.rotation = originalPelvisRotation;
    }

    private void PlayGetUpAnimation()
    {
        if (pelvis.eulerAngles.x > 0 && pelvis.eulerAngles.x < 180)
        {
            animator.SetTrigger("GetUpFront");
        }
        else
        {
            animator.SetTrigger("GetUpBack");
        }
    }

    public void AttachPelvisTo(FixedJoint joint)
    {
        pelvis.position = joint.transform.position;
        pelvis.rotation = joint.transform.rotation;

        joint.connectedBody = pelvisRigidbody;
    }
    
    public void AttachHeadTo(FixedJoint joint)
    {
        headRigidbody.position = joint.transform.position;
        headRigidbody.rotation = joint.transform.rotation;

        joint.connectedBody = headRigidbody;
    }

    public void UnAttach(FixedJoint joint)
    {
        joint.connectedBody = null;
    }

    public void DisableRagdollWithDelay(float delay)
    {
        stopRagdollCoroutine = StartCoroutine(DisableRagdollWithDelayCoroutine(delay));
    }

    private IEnumerator DisableRagdollWithDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerManager.GrabbablePlayer.isGrabbed || playerManager.isStun || !isRagdoll)
        {
            yield break;
        }

        DisableRagdoll();
    }
    
    public Rigidbody GetRandomLeg()
    {
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            return rightLegRigidbody;
        }
        else
        {
            return leftLegRigidbody;
        }
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}