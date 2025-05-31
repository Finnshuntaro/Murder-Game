using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AccuseScreenUI : MonoBehaviour
{
    public void ReturnGame()
    {
        SceneManager.UnloadSceneAsync(3); // Unload AccuseScene
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Victory()
    {
        Debug.Log("You Won");

        // Clean up previous scenes
        SceneManager.UnloadSceneAsync(2); // Unload Scene 2 (main gameplay)
        SceneManager.UnloadSceneAsync(3); // Unload AccuseScene

        // Load the win screen
        SceneManager.LoadScene(4);
    }

    public void Defeat()
    {
        Debug.Log("You Lost");

        // Clean up previous scenes
        SceneManager.UnloadSceneAsync(2); // Unload Scene 2 (main gameplay)
        SceneManager.UnloadSceneAsync(3); // Unload AccuseScene

        // Load the lose screen
        SceneManager.LoadScene(5);
    }
}
