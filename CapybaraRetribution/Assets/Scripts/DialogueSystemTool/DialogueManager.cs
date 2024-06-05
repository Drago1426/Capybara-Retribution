using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystemTool.ScriptableObjects;
using DialogueSystemTool.Data;
using TMPro;

namespace DialogueSystemTool.Editor
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        public TMP_Text dialogueText; // Use TMP_Text instead of Text
        public Transform optionsContainer;
        public Button optionButtonPrefab;
        public Canvas dialogueCanvas; // Reference to the Canvas

        private DialogueNode currentNode;
        private DialogueTree currentTree;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            dialogueCanvas.enabled = false; // Ensure the canvas is initially hidden
        }

        public void StartDialogue(DialogueTree dialogueTree)
        {
            dialogueCanvas.enabled = true; // Show the canvas when dialogue starts
            currentTree = dialogueTree;
            currentNode = currentTree.nodes[0];
            DisplayCurrentNode();
        }

        public void DisplayCurrentNode()
        {
            dialogueText.text = currentNode.dialogueText;

            foreach (Transform child in optionsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var option in currentNode.options)
            {
                var button = Instantiate(optionButtonPrefab, optionsContainer);
                button.GetComponentInChildren<TMP_Text>().text = option.optionText; // Use TMP_Text
                button.onClick.AddListener(() => OnOptionSelected(option));
            }
        }

        void OnOptionSelected(DialogueOption option)
        {
            currentNode = currentTree.GetNodeById(option.nextNodeId);
            DisplayCurrentNode();
        }

        public void EndDialogue()
        {
            dialogueText.text = "";
            foreach (Transform child in optionsContainer)
            {
                Destroy(child.gameObject);
            }

            dialogueCanvas.enabled = false; // Hide the canvas when dialogue ends
        }
    }
}