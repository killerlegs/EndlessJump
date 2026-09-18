using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    Vector3 followOffset;

    void Start()
    {
        followOffset =  transform.position - followTarget.position;
    }

    void LateUpdate()
    {
        transform.position = followTarget.position + followOffset;
    }
}
