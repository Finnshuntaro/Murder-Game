using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public DialogueManager dialogueManager; // references the dialogueManager to see the isDialogueActive state
    public PlayerMotor playerMotor;
    public Camera cam;
    private float xRotation = 0f;

    [Header("Sensitivity")]
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    [Header("Head Sway")]
    [SerializeField] private float swayAmount = 0.05f;
    [SerializeField] private float swaySpeed = 6f;

    private Vector3 defaultCamPos;
    private float swayTimer = 0f;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen and hide it

        // Saves the camera’s starting position for head sway
        if (cam != null)
            defaultCamPos = cam.transform.localPosition;
    }

    void Update()
    {
        HandleHeadSway();
    }

    public void ProcessLook(Vector2 input)
    {
        // stop movements if dialogue is happening
        if (dialogueManager != null && dialogueManager.IsDialogueActive())
            return;

        // Horizontal and Vertical look inputs
        float mouseX = input.x; // Horizontal
        float mouseY = input.y; // Vertical

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    private void HandleHeadSway()
    {
        if (playerMotor == null || cam == null) return; // Exit if required references are not set

        Vector2 moveInput = playerMotor.GetMovementInput(); // Get movement input from PlayerMotor

        // Apply sway only when moving and grounded
        if (moveInput != Vector2.zero && playerMotor.IsGrounded()) 
        {
            swayTimer += Time.deltaTime * swaySpeed; // Increment sway timer based on time and speed

            // Calculate X and Y sway using sine and cosine waves
            float swayX = Mathf.Sin(swayTimer) * swayAmount;
            float swayY = Mathf.Cos(swayTimer * 2f) * swayAmount * 0.5f;

            cam.transform.localPosition = defaultCamPos + new Vector3(swayX, swayY, 0f); // Offset the camera position by the sway amounts
        }
        else
        {
            // Reset sway and smoothly return camera to original position
            swayTimer = 0f;
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, defaultCamPos, Time.deltaTime * swaySpeed);
        }
    }
}