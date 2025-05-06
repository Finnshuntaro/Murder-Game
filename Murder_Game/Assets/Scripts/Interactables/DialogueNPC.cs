using System;
using UnityEngine;

public class DialogueNPC : Interactable
{
    public NPCScript npc;
    public DialogueManager dialogueManager;

    void Start()
    {
        if (!npc) npc = GetComponent<NPCScript>();
        if (!dialogueManager) dialogueManager = FindObjectOfType<DialogueManager>();
    }

    protected override void Interact()
    {
        if (npc != null && dialogueManager != null)
        {
            dialogueManager.StartDialogue(npc);
        }
    }
}

