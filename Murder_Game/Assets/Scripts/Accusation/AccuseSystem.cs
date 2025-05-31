using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccuseSystem : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private NPCTracker npcTracker;

    public GameObject accuseUIPanel; // This UI only becomes visible once all NPCs have been spoken to

    private bool hasActivatedUI = false; // Prevents UI from being activated repeatedly

    void Start()
    {
        // Find necessary managers in the scene
        dialogueManager = UnityEngine.Object.FindFirstObjectByType<DialogueManager>();
        npcTracker = UnityEngine.Object.FindFirstObjectByType<NPCTracker>();

        if (accuseUIPanel != null)
        {
            accuseUIPanel.SetActive(false); // Hide the panel at the beginning
        }
    }

    void Update()
    {
        // Activate UI if all NPCs have been spoken to
        if (!hasActivatedUI && npcTracker != null && npcTracker.AllNPCsSpokenTo())
        {
            if (accuseUIPanel != null)
            {
                accuseUIPanel.SetActive(true);
                hasActivatedUI = true;
            }
        }

        // 'Q' proceeds to the accusation screen
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Don't allow scene change if a dialogue is still active
            if (dialogueManager != null && dialogueManager.IsDialogueActive())
            {
                Debug.Log("You can’t proceed while a dialogue is active.");
                return;
            }

            if (npcTracker != null && npcTracker.AllNPCsSpokenTo())
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

    // Changes scene to the AccuseScene
    public void ToAccuseScene()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
        Cursor.lockState = CursorLockMode.None;
    }
}
