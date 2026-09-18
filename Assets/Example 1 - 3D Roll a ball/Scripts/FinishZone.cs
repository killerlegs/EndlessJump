using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [SerializeField] ParticleSystem finishVfx; // set manually
    [SerializeField] GameManager gameManger;


    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player == null) return;

        finishVfx.Play();
        gameManger.OnPlayerReachedFinish();
    }
}
