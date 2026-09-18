using UnityEngine;

// Runs right automatically, jumps (with an optional double jump), and
// reports obstacle collisions to the GameManager.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameManager gameManager;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundedRaycastStartPoint;

    [Header("Player Stats")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] int maxJumps = 2;
    [SerializeField] float groundCheckDistance = 0.6f;

    int jumpsRemaining; // resets to maxJumps when grounded

    void Update()
    {
        if (gameManager.IsGameOver) return;

        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);

        // Also require the player to not be moving upward - right after a jump
        // the ground is still within raycast range for a few frames, which
        // would otherwise refill jumpsRemaining instantly.
        if (IsGrounded() && rb.linearVelocity.y <= 0f)
        {
            jumpsRemaining = maxJumps;
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundedRaycastStartPoint.position, Vector2.down, groundCheckDistance);
        return hit && hit.collider.GetComponent<Obstacle>() == null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Obstacle>() != null)
        {
            gameManager.OnPlayerCollided();
        }
    }
    // Displays the grounded raycast for debug purposes
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundedRaycastStartPoint.position, Vector2.down * groundCheckDistance);
    }
}
