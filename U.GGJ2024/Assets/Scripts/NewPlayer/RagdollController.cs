using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform pelvis;
    public Rigidbody pelvisRigidbody;
    private Rigidbody[] ragdollBones;
    private Animator animator;
    private Collider playerCollider;
    private Rigidbody playerRigidbody;
    private bool isAttached = false;

    private Transform attachedTo;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        ragdollBones = rootBone.GetComponentsInChildren<Rigidbody>();
        
        pelvisRigidbody = pelvis.GetComponent<Rigidbody>();
        
        playerCollider = GetComponent<Collider>();
        playerRigidbody = GetComponent<Rigidbody>();

        InitRagdoll();
    }

    private void Update()
    {
        if (isAttached)
        {
            pelvis.position = attachedTo.position;
            pelvis.rotation = attachedTo.rotation;
        }
    }

    public void DisableRagdoll()
    {
        foreach (var ragdoll in ragdollBones)
        {
            ragdoll.isKinematic = true;
        }
        
        animator.enabled = true;
        playerCollider.enabled = true;
        playerRigidbody.useGravity = true;
        
        PlayGetUpAnimation();
        
        AlignPositionToHips();
        AlignRotationToHips();
    }

    public void EnableRagdoll()
    {
        foreach (var ragdoll in ragdollBones)
        {
            ragdoll.isKinematic = false;
        }

        playerRigidbody.useGravity = false;
        animator.enabled = false;
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
        transform.Rotate(transform.up, pelvis.eulerAngles.y); 
    }

    private void PlayGetUpAnimation()
    {
        if(pelvis.eulerAngles.x > 0 && pelvis.eulerAngles.x < 180)
        {
            animator.SetTrigger("GetUpFront");
        }
        else
        {
            animator.SetTrigger("GetUpBack");
        }
    }

    public void AttachTo(Transform transform)
    {
        isAttached = true;
        attachedTo = transform;
        pelvisRigidbody.isKinematic = true;
    }

    public void UnAttach()
    {
        isAttached = false;
        attachedTo = null;
        pelvisRigidbody.isKinematic = false;
    }

    public void DisableRagdollWithDelay(float delay)
    {
        StartCoroutine(DisableRagdollWithDelayCoroutine(delay));
    }
    
    private IEnumerator DisableRagdollWithDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableRagdoll();
    }
}
