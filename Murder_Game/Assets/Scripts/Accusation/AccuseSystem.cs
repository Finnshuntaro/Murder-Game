using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccuseSystem : MonoBehaviour
{
    public DialogueManager dialogueManager; // references the dialogueManager to see the isDialogueActive state

    // Assigning this in the inspector allows you to check each NPC 
    public NPCScript npc1; 
    public NPCScript npc2;
    public NPCScript npc3;
    public NPCScript npc4;

    public GameObject accuseUIPanel; // This UI (panel + text) only becomes visible once all NPCs have been spoken to

    private bool hasActivatedUI = false; // Prevents UI from being activated repeatedly

    void Start()
    {
        // At the beginning of the scene, the UI panel should not be visible
        if (accuseUIPanel != null)
        {
            accuseUIPanel.SetActive(false);
        }
    }

    void Update()
    {
        // If all NPCs have been spoken to, activate the UI panel (if it hasn’t been already)
        if (!hasActivatedUI && npc1.hasSpoken && npc2.hasSpoken && npc3.hasSpoken && npc4.hasSpoken)
        {
            if (accuseUIPanel != null)
            {
                accuseUIPanel.SetActive(true);
                hasActivatedUI = true; // Prevent re-triggering
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) // The Button I plan to use to continue with the Accusation screen.
        {
            // First, check if a dialogue is currently active
            if (dialogueManager != null && dialogueManager.IsDialogueActive())
            {
                Debug.Log("You can’t proceed while a dialogue is active.");
                return;
            }

            // checks to see if all NPC's have been spoken to.
            if (npc1.hasSpoken && npc2.hasSpoken)
            {
                Debug.Log("All NPCs have been spoken to!");
                ToAccuseScene();
            }
            else
            {
                Debug.Log("You still need to speak to all NPCs.");
            }
        }
    }

    // changes scene to the AccuseScene
    public void ToAccuseScene()
    {
        SceneManager.LoadScene(3);
        Cursor.lockState = CursorLockMode.None;
    }
}
