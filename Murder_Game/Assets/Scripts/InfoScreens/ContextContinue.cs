using UnityEditor.SearchService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContextContinue : MonoBehaviour
{
    public void ContContinue()
    {
        SceneManager.LoadScene(2);
    }
}