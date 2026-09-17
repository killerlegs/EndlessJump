using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float force = 10f;
    [SerializeField] Rigidbody rb; // set manually

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
        rb.AddForce(moveInput.normalized * force);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"collided {collision.gameObject}");

        if (collision.gameObject.GetComponent<KillZone>() != null)
        {
            Debug.Log($"collided with killzone {collision.gameObject}");

            Respawn();
        }
    }

    void Respawn()
    {
        rb.transform.position = startPoint.transform.position;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
}
