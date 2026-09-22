using UnityEngine;

// Physics-based ball controller: WASD/arrow input is turned into a push
// (Rigidbody.AddForce) relative to the camera's facing direction, so
// "forward" always means "away from the camera" no matter how it's rotated.
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform raycastStartPoint;
    [SerializeField] GameManager gameManager;

    [Header("Settings")]
    [SerializeField] float moveForce = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundCheckDistance = 0.6f;

    Vector3 moveInput;

    void Update()
    {
        // GetAxisRaw returns -1, 0, or 1 with no smoothing - contrary to Input.GetAxis
        Vector3 inputInCameraSpace = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical"));

        // TransformVector transforms the local direction into world space using
        // the camera's current rotation.
        moveInput = cameraTransform.TransformVector(inputInCameraSpace);

        // Remove the y component, we only want to move the ball horizontally, nomralize afterwards.
        moveInput.y = 0;
        moveInput.Normalize();

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // Jump add force is allowed to be inside the normal Update instead of the FixedUpdate,
            // because it only happens once and is not a continous addition of force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Continous physics forces belong in FixedUpdate, since it runs on physics's own
        // fixed timestep instead of once per rendered frame like Update.
        rb.AddForce(moveInput.normalized * moveForce);
    }

    // The grounded check here is a short downward raycast: if it hits anything, the ball is touching ground.
    bool IsGrounded()
    {
        return Physics.Raycast(raycastStartPoint.position, Vector3.down, groundCheckDistance);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<KillZone>() != null)
        {
            gameManager.OnPlayerEnteredKillzone();
        }
    }

    // Called by GameManager to move the ball back to the start point - used
    // both for the initial spawn and every respawn after death.
    public void OnRespawn(Vector3 startPoint)
    {
        rb.position = startPoint;
        // Without resetting these, the ball would keep its old momentum and
        // spin after being teleported back to the start.
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
