using UnityEngine;

public class PlayerGrabbing : MonoBehaviour
{
    private InputHandler inputHandler;
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("Throw Power")] [SerializeField]
    private float maxThrowPower;

    [SerializeField] private float minThrowPower;
    [SerializeField] private float throwPowerChargeSpeed;
    [HideInInspector] public bool IsCharging = false;
    private float throwPower;

    [Header("Grabbing")] public Transform grabPoint;
    [SerializeField] private float delatForInput;
    [HideInInspector] public bool IsAbleToMove;
    private PlayerGrabbing grabbedPlayer;
    private PlayerGrabbing grabbedByPlayer;
    private bool isThrowing = false;
    private bool wasThrown;
    [HideInInspector] public bool isGrabbed => grabbedByPlayer != null;
    [HideInInspector] public bool isGrabbing => grabbedPlayer != null;

    [Header("Detection")] [SerializeField] private Transform interactOrigin;
    [SerializeField] private float detectionSphereRadius;


    private void SetupDependencies()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
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
            if (playerMovement.IsGrounded)
            {
                IsAbleToMove = true;
                inputHandler.ActivateInputWithDelay(delatForInput);
                transform.rotation = new Quaternion(0, 0, 0, 0);
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
        inputHandler.DeactivateInput();
        rb.useGravity = false;
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
    }

    private void ThrowPlayer()
    {
        isThrowing = false;
        grabbedPlayer.rb.AddForce(transform.forward * throwPower, ForceMode.Impulse);
        throwPower = 0;
        grabbedPlayer.rb.useGravity = true;
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
            inputHandler.StopGamepadVibration();
        }
    }

    private void ChargePower()
    {
        if (throwPower != maxThrowPower)
        {
            inputHandler.GamepadVibrate(0.123f, 0.234f);
        }
        else
        {
            inputHandler.StopGamepadVibration();
        }

        isThrowing = true;
        throwPower += throwPowerChargeSpeed * Time.deltaTime;
        throwPower = Mathf.Clamp(throwPower, minThrowPower, maxThrowPower);
    }

    private void OnEnable()
    {
        inputHandler.OnInteractPerformed += OnInteractPerformed;
        inputHandler.OnInteractStarted += OnInteractStarted;
        inputHandler.OnInteractCanceled += OnInteractCanceled;
    }


    private void OnDisable()
    {
        inputHandler.OnInteractPerformed -= OnInteractPerformed;
        inputHandler.OnInteractStarted -= OnInteractStarted;
        inputHandler.OnInteractCanceled -= OnInteractCanceled;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactOrigin.position, detectionSphereRadius);
    }
}