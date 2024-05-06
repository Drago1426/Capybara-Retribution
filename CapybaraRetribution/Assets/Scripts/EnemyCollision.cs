using System;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {
            HealthScript.Instance.TakeDamage(1);
        }
    }
    
}
