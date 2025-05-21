using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMotor : MonoBehaviour
{
    public DialogueManager dialogueManager; // references the dialogueManager to see the isDialogueActive state

    private CharacterController controller;
    private Vector3 playerVelocity;
    public bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;

    [Header("Player Sound")]
    [SerializeField] private bool useFootsteps;

    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] carpetClips = default;
    [SerializeField] private AudioClip[] defaultClips = default;

    private float footstepTimer = 0;
    private float GetCurrentOffSet => baseStepSpeed;

    private Vector2 currentInput;
    [SerializeField] private Transform playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();  // Find and save the CharacterController on this object
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;  // Check if the player is standing on something

        Handle_Footsteps();  // Play footstep sounds if the player is moving
    }

    // Moves the player based on input from keyboard (or controller)
    public void ProcessMove(Vector2 input)
    {
        currentInput = input;  // Save the movement input (left/right, forward/back)

        // Stop moving if a dialogue is active in the game
        if (dialogueManager != null && dialogueManager.IsDialogueActive())
            return;

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;  // Move left or right
        moveDirection.z = input.y;  // Move forward or backward

        // Move the player in the correct direction and speed
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;  // Apply gravity to pull player down

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;  // Reset downward speed when player is on the ground
        }

        // Apply vertical movement like falling (or jumping, but we removed the jumping feature)
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Returns true if the player is touching the ground, false otherwise
    public bool IsGrounded()
    {
        return isGrounded;  // Give the current grounded state
    }

    // Checks if the player is moving and on the ground, then plays footstep sounds
    private void Handle_Footsteps()
    {
        if (!useFootsteps) return;  // Do nothing if footsteps are turned off
        if (!controller.isGrounded) return;  // Only play sounds if player is standing on the ground
        if (currentInput == Vector2.zero) return;  // Only play if the player is moving

        footstepTimer -= Time.deltaTime;  // Countdown timer for when next footstep sound can play

        if (footstepTimer <= 0)
        {
            // Shoot a ray down from the camera to detect what surface the player is walking on
            if (Physics.Raycast(playerCamera.position, Vector3.down, out RaycastHit hit, 3f))
            {
                // Play a sound based on the surface type found under the player
                switch (hit.collider.tag)
                {
                    case "Footsteps/CARPET":
                        footstepAudioSource.PlayOneShot(carpetClips[Random.Range(0, carpetClips.Length)]);
                        break;
                    case "Footsteps/WOOD":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length)]);
                        break;
                    case "Footsteps/DEFAULT":
                        footstepAudioSource.PlayOneShot(defaultClips[Random.Range(0, defaultClips.Length)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(defaultClips[Random.Range(0, defaultClips.Length)]);
                        break;
                }
            }

            footstepTimer = GetCurrentOffSet;  // Reset timer so footsteps don’t play too fast
        }
    }

    // Returns the current movement input, useful for other scripts that need it
    public Vector2 GetMovementInput()
    {
        return currentInput;  // Give back the current movement input vector
    }
}
