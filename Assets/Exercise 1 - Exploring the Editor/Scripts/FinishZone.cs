using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [SerializeField] ParticleSystem finishVfx; // set manually


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Player>() != null)
        {
            Debug.Log("Finish Zone Triggered");
            finishVfx.Play();
        }
    }   
}
