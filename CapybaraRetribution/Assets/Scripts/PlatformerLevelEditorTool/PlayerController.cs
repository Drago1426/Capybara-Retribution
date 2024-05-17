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

    // Private variables for audio clips
    private AudioClip jumpSound;
    private AudioClip hurtSound;
    
    // Private variables
    private Rigidbody2D rb;
    private bool facingRight = true;
    private AudioSource audioSource;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        jumpSound = Resources.Load<AudioClip>("Audio/jumpSound");
        hurtSound = Resources.Load<AudioClip>("Audio/hurtSound");
        
        if (jumpSound == null)
        {
            Debug.LogError("Failed to load jump sound from path: Resources/Audio/jumpSound.wav");
        }
        
        if (hurtSound == null)
        {
            Debug.LogError("Failed to load hurt sound from path: Resources/Audio/hurtSound.wav");
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
    
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
