using UnityEngine;

// Falls under normal gravity and climbs by wall-jumping left/right (with
// reduced gravity while clinging to a wall), reporting obstacle collisions
// to the GameManager.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameManager gameManager;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundedRaycastStartPoint;

    [Header("Player Stats")]
    [SerializeField] float jumpForce = 8f;
    [SerializeField] int maxJumps = 2;
    [SerializeField] float groundCheckDistance = 0.6f;
    [SerializeField] float wallClingGravityScale = 0.1f;

    float normalGravityScale;
    int jumpsRemaining; // resets to maxJumps when clinging to a wall

    void Start()
    {
        normalGravityScale = rb.gravityScale;
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        if (gameManager.IsGameOver) return;

        // Also require the player to not be moving away from the wall -
        // right after a jump the wall is still within raycast range for a
        // few frames, which would otherwise refill jumpsRemaining instantly.
        bool clingingLeft = IsGroundedLeft() && rb.linearVelocity.x <= 0f;
        bool clingingRight = IsGroundedRight() && rb.linearVelocity.x >= 0f;

        if (clingingLeft || clingingRight)
        {
            jumpsRemaining = maxJumps;
            rb.gravityScale = wallClingGravityScale;
        }
        else
        {
            rb.gravityScale = normalGravityScale;
        }

        if (Input.GetMouseButtonDown(0) && jumpsRemaining > 0)
        {
            // Left jump: kick up and to the left, off a wall on the right.
            rb.linearVelocity = new Vector2(-jumpForce, jumpForce);
            jumpsRemaining--;
        }
        else if (Input.GetMouseButtonDown(1) && jumpsRemaining > 0)
        {
            // Right jump: kick up and to the right, off a wall on the left.
            rb.linearVelocity = new Vector2(jumpForce, jumpForce);
            jumpsRemaining--;
        }
    }

    bool IsGroundedLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundedRaycastStartPoint.position, Vector2.left, groundCheckDistance);
        return hit && hit.collider.GetComponent<Obstacle>() == null;
    }

    bool IsGroundedRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundedRaycastStartPoint.position, Vector2.right, groundCheckDistance);
        return hit && hit.collider.GetComponent<Obstacle>() == null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Obstacle>() != null)
        {
            Die();
        }
    }

    // Called by KillZone (a trigger collider) when the player falls into it.
    public void Die()
    {
        gameManager.OnPlayerCollided();
    }

    // Displays the grounded raycasts for debug purposes
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundedRaycastStartPoint.position, Vector2.left * groundCheckDistance);
        Gizmos.DrawRay(groundedRaycastStartPoint.position, Vector2.right * groundCheckDistance);
    }
}
