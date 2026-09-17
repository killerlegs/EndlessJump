using UnityEditor.Build.Content;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float moveForce = 10f;
    [SerializeField] Rigidbody rb; // set manually
    [SerializeField] GameManager gameManager; 

    Vector3 moveInput;

    void Update()
    {
        Vector3 inputInCameraSpace = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // convert from camera space to world space
        moveInput = cameraTransform.TransformVector(inputInCameraSpace);

        // Remove the y component, we only want to move the ball horizontally
        moveInput.y = 0;
        moveInput.Normalize();
    }

    void FixedUpdate()
    {
        rb.AddForce(moveInput.normalized * moveForce);
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
