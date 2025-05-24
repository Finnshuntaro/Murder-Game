using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Use the TMPro namespace for TextMeshPro components

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Queue<string> sentences;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentSentence = ""; // Store current sentence for skipping

    public float textSpeed = 0.03f; // Delay between each character
    private bool inputOnCooldown = false;
    private float inputCooldownDuration = 0.2f;

    void Start()
    {
        sentences = new Queue<string>(); // Create the queue to hold sentences
        dialoguePanel.SetActive(false); // Hide the DialogueBox until dialogue starts
    }

    public void StartDialogue(NPCScript npc)
    {
        if (isDialogueActive) return;

        npc.hasSpoken = true; // Marks this NPC as having been spoken to

        isDialogueActive = true;
        dialoguePanel.SetActive(true); // DialogueBox appears
        nameText.text = npc.npcName; // Displays the NPC's name in the dialogue UI

        // Clear any previous dialogue and load this NPC's lines
        sentences.Clear();
        foreach (string sentence in npc.dialogueLines)
        {
            sentences.Enqueue(sentence);
        }

        StartCoroutine(InputCooldown()); // Adds a short input delay to prevent skipping the first line

        DisplayNextSentence(); 
    }

    void Update()
    {
        // Don’t allow input if dialogue isn’t active or cooldown is still happening
        if (!isDialogueActive || inputOnCooldown) return;

        // If 'E' is pressed, either skip typing or show the next line
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(InputCooldown()); // Prevent extra presses within the same frame

            if (isTyping)
            {
                StopCoroutine(typingCoroutine); // Cancel the typewriter effect
                dialogueText.text = currentSentence; // Instantly show the whole sentence
                isTyping = false;
            }
            else
            {
                DisplayNextSentence(); // Show the next sentence in the queue
            }
        }
    }

    public void DisplayNextSentence()
    {
        // If there’s nothing left, end the dialogue
        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogueAfterDelay());
            return;
        }

        currentSentence = sentences.Dequeue(); // Get the next sentence

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop any previous typing
        }

        typingCoroutine = StartCoroutine(TypeSentence(currentSentence)); // Start typing new sentence
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear current text to start fresh

        // Add one letter at a time with a small delay
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false; // Typing is finished
    }

    IEnumerator EndDialogueAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Prevent re-trigger on same frame
        isDialogueActive = false;
        dialoguePanel.SetActive(false); // Hide the DialogueBox
    }

    IEnumerator InputCooldown()
    {
        inputOnCooldown = true; // Block input temporarily
        yield return new WaitForSeconds(inputCooldownDuration); // Wait a short time
        inputOnCooldown = false; // Re-enable input
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive; // Allow other scripts to check if dialogue is ongoing
    }
}