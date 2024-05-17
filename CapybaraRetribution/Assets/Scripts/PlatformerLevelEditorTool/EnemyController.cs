using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 10; 
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Try to access the PlayerController component on the player
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                // Calculate the hit direction
                Vector2 hitDirection = (player.transform.position - transform.position).normalized;

                // Call the TakeDamage method on the player to reduce health
                player.TakeDamage(damageAmount, hitDirection);
                Debug.Log("Enemy has collided with the player and dealt damage.");
            }
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
