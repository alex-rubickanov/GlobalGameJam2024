using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool airControl = false;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius = 0.5f;
    public bool IsGrounded => Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float runSpeed = 12.0f;
    [SerializeField] private float moveSmoothTime = 0.1f;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private float jumpHeight;

    private InputHandler inputHandler;
    private Rigidbody rb;

    private Vector3 movementVector;
    private Vector3 movementVelocity;
    private Vector3 moveDampVelocity;

    public Action OnJumpStart;
    private void Awake()
    {
        SetupDependencies();
    }

    private void Start()
    {
        inputHandler.OnJump += Jump;
    }

    private void Jump()
    {
        if (!IsGrounded) return;

        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        OnJumpStart?.Invoke();
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
        Vector3 moveInput = inputHandler.GetMoveInput();
        movementVector = new Vector3(moveInput.x, 0, moveInput.y);
    }

    private void HandleMovement()
    {
        if (!airControl && !IsGrounded) return;

        float targetSpeed = inputHandler.WantsToRun && movementVector.magnitude > 0.5f ? runSpeed : walkSpeed;
        movementVelocity = Vector3.SmoothDamp(
            movementVelocity,
            movementVector * targetSpeed,
            ref moveDampVelocity,
            moveSmoothTime
        );

        rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
    }

    private void HandleRotation()
    {
        Vector3 direction = new Vector3(movementVector.x, 0, movementVector.z);
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothTime);
        }
    }

    private void SetupDependencies()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    
    public float GetVelocityMagnitude()
    {
        return movementVelocity.magnitude;
    }
}