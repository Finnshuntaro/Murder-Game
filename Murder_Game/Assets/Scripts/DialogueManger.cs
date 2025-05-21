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
    private string currentSentence = ""; // Store(s) current sentence for skipping

    public float textSpeed = 0.03f;

    // Cooldown to prevent immediate re-triggering
    private float dialogueCooldown = 0.3f;
    private float cooldownTimer = 0f;

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // Count down the cooldown timer
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (isDialogueActive && cooldownTimer <= 0f && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }

            cooldownTimer = dialogueCooldown; // Reset cooldown after input
        }
    }

    public void StartDialogue(NPCScript npc)
    {
        if (isDialogueActive || cooldownTimer > 0f) return;

        npc.hasSpoken = true;

        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        nameText.text = npc.npcName;
        sentences = new Queue<string>();

        foreach (string sentence in npc.dialogueLines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;  // Corrected here
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        cooldownTimer = dialogueCooldown; // Add short delay to avoid immediate retrigger
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}