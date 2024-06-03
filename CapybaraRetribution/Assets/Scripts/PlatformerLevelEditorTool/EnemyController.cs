using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 10;
    [SerializeField]
    private Transform[] waypoints; // Array of waypoints to walk through
    [SerializeField]
    private float moveSpeed = 2f; // Speed of movement
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private int health = 100; // Enemy health

    private int waypointIndex = 1; // Index of the current waypoint
    private bool isFlipped = false;
    private bool isDead = false;
    
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        Move();
    }
    
    void Move()
    {
        // Move towards the next waypoint
        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the waypoint
        if (Vector2.Distance(transform.position, waypoints[waypointIndex].transform.position) < 0.1f)
        {
            // Flip the sprite vertically
            FlipSprite();
            
            // Go to the next waypoint
            waypointIndex++;

            // Loop back to the first waypoint if at the end of the array
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Try to access the PlayerController component on the player
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                PlaySound(hitSound);
                // Calculate the hit direction
                Vector2 hitDirection = (player.transform.position - transform.position).normalized;

                // Call the TakeDamage method on the player to reduce health
                player.TakeDamage(damageAmount, hitDirection);
                Debug.Log("Enemy has collided with the player and dealt damage.");
            }
        }
    }
    
    void FlipSprite()
    {
        // Flip the sprite by inverting the y scale
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // Toggle the flip state
        isFlipped = !isFlipped;
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            // Check if the audio source is already playing a sound
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;

            // Disable enemy movement
            moveSpeed = 0;

            // Make the enemy walk-through
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            // Turn the enemy red
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
            }

            // Play the death sound
            PlaySound(deathSound);

            // Destroy the enemy after the death sound finishes playing
            Destroy(gameObject, deathSound.length);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the gizmo color
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }
}
