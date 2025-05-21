using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AccuseSystem : MonoBehaviour
{
    // Assigning this in the inspector allows you to check each NPC 
    public NPCScript npc1; 
    public NPCScript npc2; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // The Button I plan to use to continue with the Accusation screen.
        {
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
