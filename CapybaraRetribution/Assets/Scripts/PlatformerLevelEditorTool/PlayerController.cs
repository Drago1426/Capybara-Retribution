using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
    [SerializeField]
    private float attackRange = 1f;

    // Private variables for audio clips
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip hurtSound;

    // Reference to the TextMeshPro UI element
    [SerializeField]
    private TextMeshProUGUI healthText;

    // Private variables
    private Rigidbody2D rb;
    private bool facingRight = true;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private int currentHealth;
    private Animator animator;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get the Animator component from the child GameObject
        animator = GetComponentInChildren<Animator>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        currentHealth = maxHealth;
        UpdateHealthUI();

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

        // Update the animator with movement information
        if (animator != null)
        {
            animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.1f);
        }

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

        if (Input.GetKeyDown(KeyCode.X))
        {
            DestroyEnemyInFront();
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
            // Load Game Over Scene
            SceneManager.LoadScene("GameOver");
        }

        // Update health UI
        UpdateHealthUI();
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

    private void DestroyEnemyInFront()
    {
        // Determine the direction the player is facing
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        // Offset the raycast origin to avoid hitting the player itself
        float offset = 1.5f; // Adjust this value as needed
        Vector2 rayOrigin = (Vector2)transform.position + direction * offset;

        // Perform a raycast to detect enemies in front of the player
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, attackRange);

        // Debug the raycast
        Debug.DrawRay(rayOrigin, direction * attackRange, Color.red, 0.5f);

        // Check if the raycast hit an enemy
        if (hit.collider != null)
        {
            Debug.Log($"Raycast hit: {hit.collider.name}, Tag: {hit.collider.tag}");

            if (hit.collider.CompareTag("Enemy"))
            {
                // Try to get the EnemyController component
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    // Call the Die method on the enemy
                    enemy.Die();
                    Debug.Log("Enemy destroyed.");
                }
                else
                {
                    // If no EnemyController is found, destroy the GameObject as a fallback
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Enemy destroyed (fallback).");
                }
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }
}
