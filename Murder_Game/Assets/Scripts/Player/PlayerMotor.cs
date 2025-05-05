using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMotor : MonoBehaviour
{
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
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        Handle_Footsteps();
    }

    public void ProcessMove(Vector2 input)
    {
        currentInput = input;

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);

    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void Handle_Footsteps()
    {
        if (!useFootsteps) return;
        if (!controller.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(playerCamera.position, Vector3.down, out RaycastHit hit, 3f))
            {
                Debug.Log("Raycast hit: " + hit.collider.name + ", tag: " + hit.collider.tag);

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

            footstepTimer = GetCurrentOffSet; 
        }
    }
}
