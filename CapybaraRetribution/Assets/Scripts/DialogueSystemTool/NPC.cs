using DialogueSystemTool.Editor;
using DialogueSystemTool.ScriptableObjects;
using UnityEngine;

namespace DialogueSystemTool
{
    public class NPC : MonoBehaviour
    {
        public DialogueTree dialogueTree;
        private bool isPlayerInRange = false;

        void Update()
        {
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.Instance.StartDialogue(dialogueTree);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
                // Optionally show UI prompt to press E
                Debug.Log("Player in range");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
                // Optionally hide UI prompt
                Debug.Log("Player out of range");
            }
        }
    }
}