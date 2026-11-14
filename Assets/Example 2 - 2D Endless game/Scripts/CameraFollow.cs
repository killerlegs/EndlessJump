using UnityEngine;

// Follows a target at a fixed offset, using either SmoothDamp or Lerp easing.
public class CameraFollow : MonoBehaviour
{
    enum FollowMode { SmoothDamp, Lerp }

    [SerializeField] Transform target;
    [SerializeField] FollowMode followMode;

    [Header("SmoothDamp")]
    [SerializeField] float smoothTime = 0.3f;

    [Header("Lerp")]
    [SerializeField] float lerpSpeed;
    [Header("Camera Framing")]
    [SerializeField] float verticalLookAhead = 2f;

    [Header("Auto Scrolling")]
    [SerializeField] float initialScrollSpeed = 0.5f;
    [SerializeField] float scrollAcceleration = 0.03f;
    [SerializeField] float maxScrollSpeed = 2f;

    Vector3 offset;
    Vector3 velocity;
    float scrollSpeed;
    float autoScrollY;

    void Start()
    {
        // Determine offset based on initial positions
        offset = transform.position - target.position;
        offset.y += verticalLookAhead;

        scrollSpeed = initialScrollSpeed;
        autoScrollY = transform.position.y;
    }

   void LateUpdate()
    {
        if (GameManager.Instance.IsGameOver) return;

        // Increase scrolling speed gradually
        scrollSpeed = Mathf.Min(
            scrollSpeed + scrollAcceleration * Time.deltaTime,
            maxScrollSpeed
        );

        // Move camera upward automatically
        autoScrollY += scrollSpeed * Time.deltaTime;

        // Original camera following logic
        Vector3 targetPosition = target.position + offset;
        Vector3 followPosition;

        if (followMode == FollowMode.SmoothDamp)
        {
            followPosition = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                smoothTime
            );
        }
        else
        {
            followPosition = Vector3.Lerp(
                transform.position,
                targetPosition,
                lerpSpeed * Time.deltaTime
            );
        }

        // Camera can move upward automatically,
        // but it cannot move below its auto-scroll position.
        float newY = Mathf.Max(
            autoScrollY,
            followPosition.y,
            transform.position.y
        );

        transform.position = new Vector3(
            followPosition.x,
            newY,
            transform.position.z
        );
    }
}
