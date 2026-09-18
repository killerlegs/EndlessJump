using UnityEngine;

// Sits on a trigger collider at the end of the level. Ends the level if player entersit
public class FinishZone : MonoBehaviour
{
    [SerializeField] ParticleSystem finishVfx; // set manually
    [SerializeField] GameManager gameManger;

    private void OnTriggerEnter(Collider other)
    {
        // GetComponentInParent handles the Player script being on a parent
        // object of whatever collider actually touched the trigger.
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player == null) return;

        finishVfx.Play();
        gameManger.OnPlayerReachedFinish();
    }
}
