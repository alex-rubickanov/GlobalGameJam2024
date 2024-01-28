using System;
using UnityEngine;

public class NPlayerMovement : MonoBehaviour
{
    private NPlayerManager playerManager;
    private Rigidbody rb;

    [SerializeField] private bool airControl = false;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius = 0.5f;

    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float runSpeed = 12.0f;
    [SerializeField] private float moveSmoothTime = 0.1f;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private float jumpHeight = 5.0f;
    [SerializeField] private float checkGroundRayLength = 0.5f; 


    private Vector3 movementVector;
    private Vector3 movementVelocity;
    private Vector3 moveDampVelocity;

    public bool canMove = true;
    public bool isBucket = false;
    public bool isDrunk = false;
    public bool isFart = false;

    [SerializeField] private float randomDirectionDrunkTimer = 3.0f;
    private float timer;
    
    public Action OnJumpStart;
    public bool IsGrounded => Physics.CheckSphere(groundCheck.position, groundCheckRadius);

    private void Start()
    {
        SetupDependencies();
        SubscribeToEvents();

        timer = randomDirectionDrunkTimer;
    }


    private void Update()
    {
        HandleInput();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }


    private void HandleInput()
    {
        Vector3 moveInput = playerManager.InputHandler.GetMoveInput();
        if (!isBucket)
        {
            movementVector = new Vector3(moveInput.x, 0, moveInput.y);
        }
        else
        {
            movementVector = new Vector3(-moveInput.y, 0, -moveInput.x);
        }
    }

    private void HandleMovement()
    {
        bool isStayOnAnything = Physics.Raycast(transform.position, Vector3.down, checkGroundRayLength);
        if (!airControl && !IsGrounded && !isStayOnAnything) return;

        float targetSpeed = playerManager.InputHandler.WantsToRun && movementVector.magnitude > 0.5f
            ? runSpeed
            : walkSpeed;

        if (isDrunk)
        {
            targetSpeed = targetSpeed / 2;
        }

        if (isFart)
        {
            targetSpeed = targetSpeed * 2;
        }
        
        movementVelocity = Vector3.SmoothDamp(
            movementVelocity,
            movementVector * targetSpeed,
            ref moveDampVelocity,
            moveSmoothTime
        );

        if (rb.isKinematic) return;
        if (!canMove) return;
        rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
    }

    private void HandleRotation()
    {
        if(!canMove) return;
        Vector3 direction = new Vector3(movementVector.x, 0, movementVector.z);
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothTime);
        }
    }

    private void Jump()
    {
        if (!IsGrounded) return;

        rb.velocity += new Vector3(0, jumpHeight, 0);
        OnJumpStart?.Invoke();
    }
    private void SetupDependencies()
    {
        playerManager = GetComponentInParent<NPlayerManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void SubscribeToEvents()
    {
        playerManager.InputHandler.OnJump += Jump;
    }

    public float GetVelocityMagnitude()
    {
        return rb.velocity.magnitude;
    }

    private void OnDrawGizmos()
    {
        if (IsGrounded)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        
        Gizmos.DrawRay(transform.position, Vector3.down * checkGroundRayLength);
    }
}