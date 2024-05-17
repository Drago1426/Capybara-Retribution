using UnityEngine;

public class TrapBehavior : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player triggered the trap
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                Debug.Log("Trap activated!");

                // Calculate the hit direction from the trap to the player
                Vector2 hitDirection = (player.transform.position - transform.position).normalized;
                
                // Apply damage to the player
                player.TakeDamage(damageAmount, hitDirection);
            }
        }
    }
}
