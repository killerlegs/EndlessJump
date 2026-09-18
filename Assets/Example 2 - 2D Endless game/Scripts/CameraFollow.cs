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

    Vector3 offset;
    Vector3 velocity;

    void Start()
    {
        // Determine offset based on initial positions
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (followMode == FollowMode.SmoothDamp)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position, target.position + offset, ref velocity, smoothTime
            );
        }
        else if (followMode == FollowMode.Lerp)
        {
            transform.position = Vector3.Lerp(
                transform.position, target.position + offset, lerpSpeed * Time.deltaTime
            );
    }
}
