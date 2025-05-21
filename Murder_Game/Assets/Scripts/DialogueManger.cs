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
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(NPCScript npc)
    {
        if (isDialogueActive) return;

        npc.hasSpoken = true; // Mark that this NPC has been interacted with

        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        nameText.text = npc.npcName;

        sentences.Clear();
        foreach (string sentence in npc.dialogueLines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    void Update()
    {
        if (!isDialogueActive || inputOnCooldown) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(InputCooldown());

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
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogueAfterDelay());
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
            dialogueText.text += letter; // Fixed line
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    IEnumerator EndDialogueAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Prevent re-trigger on same frame
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }

    IEnumerator InputCooldown()
    {
        inputOnCooldown = true;
        yield return new WaitForSeconds(inputCooldownDuration);
        inputOnCooldown = false;
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}