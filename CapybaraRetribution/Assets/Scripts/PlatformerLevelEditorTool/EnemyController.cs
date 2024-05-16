using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int damageAmount = 10;  // Amount of damage this enemy deals to the player

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Try to access the PlayerController component on the player
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                // Call the TakeDamage method on the player to reduce health
                player.TakeDamage(damageAmount);
                Debug.Log("Enemy has collided with the player and dealt damage.");
            }
        }
    }
}
