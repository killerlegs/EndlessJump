using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float moveForce = 10f;
    [SerializeField] Rigidbody rb; // set manually
    [SerializeField] GameManager gameManager;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundCheckDistance = 0.6f;

    Vector3 moveInput;

    void Update()
    {
        Vector2 moveAxis = Vector2.zero;

        // A bit of a dirty workaround to get the movement keys, so students don't need to change input settings.
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveAxis.x -= 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveAxis.x += 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveAxis.y -= 1;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveAxis.y += 1;
        }

        Vector3 inputInCameraSpace = new Vector3(moveAxis.x, 0, moveAxis.y);

        // convert from camera space to world space
        moveInput = cameraTransform.TransformVector(inputInCameraSpace);

        // Remove the y component, we only want to move the ball horizontally
        moveInput.y = 0;
        moveInput.Normalize();

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            // Jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Move horizontally
        rb.AddForce(moveInput.normalized * moveForce);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<KillZone>() != null)
        {
            gameManager.HandlePlayerDeath();
        }
    }

    public void OnRespawn(Vector3 startPoint)
    {
        transform.position = startPoint;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
}
