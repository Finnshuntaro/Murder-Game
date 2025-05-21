using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public DialogueManager dialogueManager; // references the dialogueManager to see the isDialogueActive state

    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30;
    public float yensitivity = 30;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ProcessLook(Vector2 input)
    {
        // stop movements if dialogue is happening
        if (dialogueManager != null && dialogueManager.IsDialogueActive())
            return; 

        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * yensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}