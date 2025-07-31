using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float mouseSensitivity = 2.0f;
    public float gravity = -9.81f; // Standard gravity

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    // Reference to the CameraHolder, so we can rotate it
    public Transform cameraHolder;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Lock and hide the cursor for proper FPS control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- Movement ---
        float x = Input.GetAxis("Horizontal"); // A/D keys
        float z = Input.GetAxis("Vertical");   // W/S keys

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // --- Gravity ---
        // Apply gravity if not grounded (CharacterController handles grounding implicitly)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset velocity.y if grounded to prevent continuous acceleration downwards
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        // --- Camera Look (Mouse) ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the Player GameObject (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the CameraHolder (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp vertical rotation
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}