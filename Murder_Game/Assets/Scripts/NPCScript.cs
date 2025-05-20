using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public string npcName;  // The name of the NPC
    public string[] dialogueLines;  // An array of strings that holds the NPC's dialogue lines

    public bool hasSpoken = false; // Ensures that at the start of the scene, no NPC is marked as spoken to
}
