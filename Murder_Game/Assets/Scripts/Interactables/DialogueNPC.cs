using System;
using UnityEngine;

public class DialogueNPC : Interactable
{
    public NPCScript npc;
    private DialogueManager dialogueManager;

    void Start()
    {
        if (!npc) npc = GetComponent<NPCScript>(); // Get the attached NPCScript

        dialogueManager = UnityEngine.Object.FindFirstObjectByType<DialogueManager>(); // Use modern method to find the DialogueManager
    }

    // Called when the player interacts with this NPC
    protected override void Interact()
    {
        // If everything is set up, start the dialogue using this NPC's data
        if (npc != null && dialogueManager != null)
        {
            dialogueManager.StartDialogue(npc);
        }
    }
}

