using UnityEngine;

// Physics-based ball controller: WASD/arrow input is turned into a push
// (Rigidbody.AddForce) relative to the camera's facing direction, so
// "forward" always means "away from the camera" no matter how it's rotated.
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
        // GetAxisRaw returns -1, 0, or 1 with no smoothing, so input reacts instantly.
        Vector3 inputInCameraSpace = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical"));

        // TransformVector rotates that local direction into world space using
        // the camera's current rotation, without applying its position or scale.
        moveInput = cameraTransform.TransformVector(inputInCameraSpace);

        // Remove the y component, we only want to move the ball horizontally
        moveInput.y = 0;
        moveInput.Normalize();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // Jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Physics forces belong in FixedUpdate, since it runs on physics's own
        // fixed timestep instead of once per rendered frame like Update.
        rb.AddForce(moveInput.normalized * moveForce);
    }

    // A short downward raycast: if it hits anything, the ball is touching ground.
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

    // Called by GameManager to move the ball back to the start point - used
    // both for the initial spawn and every respawn after death.
    public void OnRespawn(Vector3 startPoint)
    {
        transform.position = startPoint;
        // Without resetting these, the ball would keep its old momentum and
        // spin after being teleported back to the start.
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
}
