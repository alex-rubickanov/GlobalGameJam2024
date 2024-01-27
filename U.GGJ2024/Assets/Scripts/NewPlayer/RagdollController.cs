using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private NPlayerManager playerManager;

    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform pelvis;
    public Rigidbody pelvisRigidbody;
    private Animator animator;
    private Collider playerCollider;
    private Rigidbody playerRigidbody;

    private Rigidbody[] ragdollBones;
    [HideInInspector] public Collider[] ragdollColliders;

    private void Start()
    {
        playerManager = GetComponentInParent<NPlayerManager>();
        animator = GetComponentInChildren<Animator>();

        ragdollBones = rootBone.GetComponentsInChildren<Rigidbody>();
        ragdollColliders = rootBone.GetComponentsInChildren<Collider>();

        pelvisRigidbody = pelvis.GetComponent<Rigidbody>();

        playerCollider = GetComponent<Collider>();
        playerRigidbody = GetComponent<Rigidbody>();

        InitRagdoll();
    }

    public void DisableRagdoll()
    {
        playerRigidbody.isKinematic = false;
        
        foreach (var ragdoll in ragdollBones)
        {
            ragdoll.isKinematic = true;
        }

        playerCollider.enabled = true;


        AlignRotationToHips();
        AlignPositionToHips();

        animator.enabled = true;
        PlayGetUpAnimation();
        
        playerManager.InputHandler.ActivateInput();
    }

    public void EnableRagdoll()
    {
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

    public void AttachTo(FixedJoint joint)
    {
        pelvis.position = joint.transform.position;
        pelvis.rotation = joint.transform.rotation;
        
        joint.connectedBody = pelvisRigidbody;
    }

    public void UnAttach(FixedJoint joint)
    {
        joint.connectedBody = null;
    }

    public void DisableRagdollWithDelay(float delay)
    {
        StopCoroutine(DisableRagdollWithDelayCoroutine(delay));
        StartCoroutine(DisableRagdollWithDelayCoroutine(delay));
    }

    private IEnumerator DisableRagdollWithDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableRagdoll();
    }
}
