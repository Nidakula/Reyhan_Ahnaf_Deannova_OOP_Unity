using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerMovement class
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 maxSpeed;
    [SerializeField] private Vector2 timeToFullSpeed;
    [SerializeField] private Vector2 timeToStop;
    [SerializeField] private Vector2 stopClamp;
    [SerializeField] private Vector2 minBoundary;
    [SerializeField] private Vector2 maxBoundary;

    private Vector2 moveDirection;
    private Vector2 moveVelocity;
    private Vector2 moveFriction;
    private Vector2 stopFriction;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Kalkulasi kecepatan dan friksi
        moveVelocity = new Vector2(
            2f * maxSpeed.x / timeToFullSpeed.x,
            2f * maxSpeed.y / timeToFullSpeed.y
        );

        moveFriction = new Vector2(
            -2f * maxSpeed.x / (timeToFullSpeed.x * timeToFullSpeed.x),
            -2f * maxSpeed.y / (timeToFullSpeed.y * timeToFullSpeed.y)
        );

        stopFriction = new Vector2(
            -2f * maxSpeed.x / (timeToStop.x * timeToStop.x),
            -2f * maxSpeed.y / (timeToStop.y * timeToStop.y)
        );

        // Menambah lebar boundary sebesar 2 unit dan tinggi sebesar 1 unit
        AdjustBoundary(8.6f, 8.6f, 4.4f, 5f);
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        Vector2 currentVelocity = rb.velocity;
        Vector2 friction = GetFriction();

        // Horizontal movement
        if (horizontalInput != 0)
        {
            float targetSpeedX = moveDirection.x * maxSpeed.x;
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetSpeedX, moveVelocity.x * Time.fixedDeltaTime);
        }
        else
        {
            // Apply friction
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, -friction.x * Time.fixedDeltaTime);
        }

        // Vertical movement
        if (verticalInput != 0)
        {
            float targetSpeedY = moveDirection.y * maxSpeed.y;
            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, targetSpeedY, moveVelocity.y * Time.fixedDeltaTime);
        }
        else
        {
            // Apply friction
            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, 0, -friction.y * Time.fixedDeltaTime);
        }

        rb.velocity = currentVelocity;

        // Boundary check
        Vector2 newPosition = rb.position;
        newPosition.x = Mathf.Clamp(newPosition.x, minBoundary.x, maxBoundary.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBoundary.y, maxBoundary.y);
        rb.position = newPosition;
    }

    public Vector2 GetFriction()
    {
        Vector2 friction = Vector2.zero;
        friction.x = moveDirection.x != 0 ? moveFriction.x : stopFriction.x;
        friction.y = moveDirection.y != 0 ? moveFriction.y : stopFriction.y;
        return friction;
    }

    public bool IsMoving()
    {
        return Mathf.Abs(rb.velocity.x) > stopClamp.x || 
               Mathf.Abs(rb.velocity.y) > stopClamp.y;
    }

    // Metode untuk menyesuaikan boundary
    public void AdjustBoundary(float leftAdjustment, float rightAdjustment, float topAdjustment, float bottomAdjustment)
    {
        minBoundary.x -= leftAdjustment;
        maxBoundary.x += rightAdjustment;
        minBoundary.y -= bottomAdjustment;
        maxBoundary.y += topAdjustment;
    }
}