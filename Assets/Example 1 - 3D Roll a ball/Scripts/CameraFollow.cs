using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 offset;
    public enum FollowMode { SmoothDamp, Lerp }
    [SerializeField] FollowMode followMode;

    [Header("SmoothDamp Values")]
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    [Header("Lerp Values")]
    public float lerpSpeed;

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
}