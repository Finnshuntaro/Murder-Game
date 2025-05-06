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

    public float textSpeed = 0.03f; // You can tweak this

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(NPCScript npc)
    {
        if (isDialogueActive) return;

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

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.F))
        {
            if (isTyping)
            {
                // Skip typing and finish sentence immediately
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

    string currentSentence = ""; // Store current sentence for skipping

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
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}