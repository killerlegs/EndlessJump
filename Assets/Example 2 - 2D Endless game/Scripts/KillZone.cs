using UnityEngine;

// A trigger collider that ends the game when the player falls into it (see
// WorldGenerator, which creates and repositions this to trail below the
// lowest loaded chunk). The check lives here rather than on PlayerController
// since this is the side with the trigger collider.
public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Die();
        }
    }
}
