using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Serialized fields for movement parameters
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float bounceBackForce = 5f;
    [SerializeField]
    private float flashDuration = 0.1f;
    [SerializeField]
    private int numberOfFlashes = 5;
    [SerializeField]
    private int maxHealth = 100;

    // Private variables for audio clips
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip hurtSound;
    
    // Private variables
    private Rigidbody2D rb;
    private bool facingRight = true;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private int currentHealth;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        currentHealth = maxHealth;
        
        // Set Rigidbody2D collision detection to Continuous
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Assign Physics Material 2D with zero friction to avoid getting stuck
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.sharedMaterial = new PhysicsMaterial2D { friction = 0, bounciness = 0 };
        }
    }

    void Update()
    {
        // Get horizontal input
        float moveInput = Input.GetAxis("Horizontal");

        // Move the player
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip the player based on the direction of movement
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        // Jump when the spacebar is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            PlaySound(jumpSound);
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
        PlaySound(hurtSound);

        // Apply bounce back force
        rb.velocity = Vector2.zero; // Reset velocity to make the bounce back consistent
        rb.AddForce(hitDirection.normalized * bounceBackForce, ForceMode2D.Impulse);

        // Start the flashing effect
        StartCoroutine(Flash());

        // Check for health depletion
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");
            // Implement what happens when the player's health reaches 0
        }
    }
    
    private IEnumerator Flash()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0); // Set sprite to transparent
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = new Color(1, 1, 1, 1); // Set sprite to opaque
            yield return new WaitForSeconds(flashDuration);
        }
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

