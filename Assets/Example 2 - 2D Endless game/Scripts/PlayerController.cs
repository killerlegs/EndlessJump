using System.Diagnostics.CodeAnalysis;
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
    [SerializeField] float jumpForceSide = 8f;
    [SerializeField] float jumpForceUp = 12f;
    [SerializeField] int maxJumps = 2;
    [SerializeField] float defaultGravityScale = 2f;
    [SerializeField] float wallClingGravityScale = 0.1f;
    [SerializeField] float groundCheckDistance = 0.6f;
    [Tooltip("Set it to ignore the player layer")]
    [SerializeField] LayerMask groundedCheckLayerMask;
    [Tooltip("Tolerance for the 'moving away from wall' velocity check, to absorb tiny residual velocities the physics solver leaves after resolving a wall collision.")]
    [SerializeField] float wallStickVelocityTolerance = 0.5f;

    int jumpsRemaining; // resets to maxJumps when clinging to a wall
    bool wasGroundedLastFrame;

    void Start()
    {
        jumpsRemaining = maxJumps;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (gameManager.IsGameOver) return;

        // We check against wallStickVelocityTolerance, as right after a jump the wall is still be within raycast range for a
        // few frames, however we dont want the player to be grounded right after jumping.
        bool clingingLeft = IsGrounded(Vector2.left) && rb.linearVelocity.x <= wallStickVelocityTolerance;
        bool clingingRight = IsGrounded(Vector2.right) && rb.linearVelocity.x >= -wallStickVelocityTolerance;

        // Reset the jump counter and limit gravity when clinging to a wall.
        if (clingingLeft || clingingRight)
        {
            rb.gravityScale = wallClingGravityScale;

            // Only reset the velocity and max jump when we started clinign to a wall.
            if (!wasGroundedLastFrame)
            {
                jumpsRemaining = maxJumps;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                wasGroundedLastFrame = true;
            }
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
            wasGroundedLastFrame = false;
        }

        if ( !clingingLeft && Input.GetMouseButtonDown(0) && jumpsRemaining > 0)
        {
            // Left jump: kick up and to the left, off a wall on the right.
            rb.linearVelocity = new Vector2(-jumpForceSide, jumpForceUp);
            jumpsRemaining--;
        }
        else if (!clingingRight && Input.GetMouseButtonDown(1) && jumpsRemaining > 0)
        {
            // Right jump: kick up and to the right, off a wall on the left.
            rb.linearVelocity = new Vector2(jumpForceSide, jumpForceUp);
            jumpsRemaining--;
        }
    }

    bool IsGrounded(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(groundedRaycastStartPoint.position, direction, groundCheckDistance, groundedCheckLayerMask);
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
        rb.gravityScale = defaultGravityScale;
        // add a small rotation force to show loss of control
        rb.freezeRotation = false;
        rb.AddTorque(25f);
        gameManager.OnPlayerCollidedWithObstacle();
    }

    // Displays the grounded raycasts for debug purposes
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundedRaycastStartPoint.position, Vector2.left * groundCheckDistance);
        Gizmos.DrawRay(groundedRaycastStartPoint.position, Vector2.right * groundCheckDistance);
    }
}
