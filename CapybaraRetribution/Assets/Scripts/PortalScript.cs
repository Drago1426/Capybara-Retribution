using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PortalScript : MonoBehaviour
{
    [SerializeField]
    private string sceneNameToLoad; // The name of the scene to load
    private bool playerInRange = false; // Flag to check if the player is in range of the GameObject

    private void Update()
    {
        // Check if the player is in range and the E key is pressed
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            LoadScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the collider is tagged as "Player"
        {
            playerInRange = true; // Set the flag to true when the player enters the trigger
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the collider is tagged as "Player"
        {
            playerInRange = false; // Reset the flag when the player exits the trigger
        }
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneNameToLoad))
        {
            SceneManager.LoadScene(sceneNameToLoad); // Load the specified scene
        }
        else
        {
            Debug.LogError("Scene name is not set for " + gameObject.name);
        }
    }
}