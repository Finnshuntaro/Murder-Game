using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AccuseScreenUI : MonoBehaviour
{
    public void ReturnGame()
    {
        SceneManager.LoadScene(2);
    }

    public void Victory()
    {
        Debug.Log("You Won");
        SceneManager.LoadScene(4);
    }

    public void Defeat()
    {
        Debug.Log("You Lost");
        SceneManager.LoadScene(5);
    }
}
