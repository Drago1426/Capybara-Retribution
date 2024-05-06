using UnityEngine;

public class TrapBehavior : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player triggered the trap
        {
            Debug.Log("Trap activated!");
            // Add logic for what happens when the trap is triggered
            // e.g., apply damage, play a sound, show an animation, etc.
        }
    }
}
