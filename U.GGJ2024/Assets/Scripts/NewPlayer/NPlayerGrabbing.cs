using System;
using UnityEngine;

public class NPlayerGrabbing : MonoBehaviour
{
    public NPlayerManager playerManager;
    private Rigidbody rb;
    private Collider col;

    [Header("Throw Power")] [SerializeField]
    private float maxThrowPower;

    [SerializeField] private float minThrowPower;
    [SerializeField] private float throwPowerChargeSpeed;
    [HideInInspector] public bool IsCharging = false;
    private float throwPower;

    [Header("Grabbing")]
    [SerializeField] public Transform grabPoint;
    [SerializeField] private float delatForInput;
    [HideInInspector] public bool IsAbleToMove;
    
    private GrabbableObject grabbedObject;
    
    private bool isThrowing = false;
    [HideInInspector] public bool isGrabbing;

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
        if (IsCharging)
        {
            ChargePower();
        }

        if (isGrabbing == false)
        {
            playerManager.InputHandler.StopGamepadVibration();
        }
    }

    private void TryGrab()
    {
        Collider[] results;
        results = Physics.OverlapSphere(interactOrigin.position, detectionSphereRadius);
        foreach (var result in results)
        {
            if (result == GetComponent<Collider>()) continue;
            if (grabbedObject) return;
            
            
            
            if (result.GetComponentInParent<GrabbableObject>())
            {
                grabbedObject = result.GetComponentInParent<GrabbableObject>();
                if (grabbedObject.canBeGrabbed)
                {
                    isGrabbing = true;
                    if (grabbedObject is GrabbablePlayer)
                    {
                        NPlayerManager grabbedPlayerManager = grabbedObject.GetComponentInParent<NPlayerManager>();
                        if (grabbedPlayerManager.PlayerGrabbing.isGrabbing)
                        {
                            grabbedPlayerManager.PlayerGrabbing.LooseObject();
                        }
                    }
                    grabbedObject.Grab(this);
                }
                else
                {
                    grabbedObject = null;
                }
            }
        }
    }

    private void ThrowObject(Vector3 force)
    {
        isGrabbing = false;
        isThrowing = false;
        if (grabbedObject)
        {
            grabbedObject.Throw(force);
        }
        
        throwPower = 0;
        grabbedObject = null;
    }

    private void OnInteractPerformed()
    {
        if (!isGrabbing)
        {
            TryGrab();
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
            ThrowObject(transform.forward * throwPower);
        }
    }

    private void ChargePower()
    {
        if (throwPower != maxThrowPower)
        {
            playerManager.InputHandler.GamepadVibrate();
        }
        else
        {
            playerManager.InputHandler.StopGamepadVibration();
        }
        isThrowing = true;
        throwPower += throwPowerChargeSpeed * Time.deltaTime;
        throwPower = Mathf.Clamp(throwPower, minThrowPower, maxThrowPower);
    }

    public void LooseObject()
    {
        isThrowing = false;

        if (grabbedObject)
        {
            grabbedObject.Throw(transform.forward);
        }
        
        throwPower = 0;
        grabbedObject = null;
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
