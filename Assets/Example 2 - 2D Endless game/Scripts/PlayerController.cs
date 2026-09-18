using UnityEngine;

// Chrome-Dino-style controller: the player runs to the right on its own and
// the camera follows it (see CameraFollow.cs), instead of the world
// scrolling past a static player/camera. Jump is a hybrid physics move (set
// linearVelocity.y directly, see lecture "Moving Rigidbodies: set velocity"),
// grounding is checked with a raycast (same pattern as Example 1's
// Player.IsGrounded()).
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

    // Internal fields
    int jumpsRemaining; // Represents how many jumps are available, resets to max jumps everytime we touch the ground


    void Update()
    {
        if (gameManager.IsGameOver) return;

        // Keep running at a constant speed to the right regardless of collisions
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);


        // Check rb.linearVelocity.y because: right after a jump
        // the ground is still within raycast range for a few 
        // frames and would reset the player jump count
        if (IsGrounded() && rb.linearVelocity.y <= 0f)
        {
            jumpsRemaining = maxJumps;
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);  // instantly add jump velocity
            jumpsRemaining--;
        }
    }

    bool IsGrounded()
    {
        // A Raycasts projects a ray from a point, in this case downwards from the player, and returns if any other collider was hit on the way.
        RaycastHit2D groundHit = Physics2D.Raycast(groundedRaycastStartPoint.position, Vector2.down, groundCheckDistance);
        if (groundHit)
        {
            bool onObstacle = groundHit.collider.gameObject.GetComponent<Obstacle>() != null;
            if (onObstacle) return false;

            Debug.Log("Grounded on: " + groundHit.collider.name, groundHit.collider);
            return true;
        }

        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Obstacle>() != null)
        {
            gameManager.OnPlayerCollided();
        }
    }

    // Displays the grounded raycast
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundedRaycastStartPoint.position, Vector2.down * groundCheckDistance);
    }
}
