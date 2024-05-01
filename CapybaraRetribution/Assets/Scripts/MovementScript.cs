using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private string groundTag = "Platform";  // Replace LayerMask with a tag for ground
    private bool jumpRequested = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is touching the ground using tags.
        isGrounded = CheckGrounded();
        
        // Handle jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Move the player left and right.
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Apply jump force if requested
        if (jumpRequested)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequested = false; // Reset jump request
        }
    }

    private bool CheckGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag(groundTag))
            {
                return true;
            }
        }
        return false;
    }
}