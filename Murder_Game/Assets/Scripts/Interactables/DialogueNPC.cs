using System;
using UnityEngine;

public class DialogueNPC : Interactable
{
    public NPCScript npc;
    private DialogueManager dialogueManager;

    void Start()
    {
        // Get the attached NPCScript
        if (!npc) npc = GetComponent<NPCScript>();

        // Use modern method to find the DialogueManager
        dialogueManager = UnityEngine.Object.FindFirstObjectByType<DialogueManager>();
    }

    protected override void Interact()
    {
        if (npc != null && dialogueManager != null)
        {
            dialogueManager.StartDialogue(npc);
        }
    }
}

