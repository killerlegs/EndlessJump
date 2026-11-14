using UnityEngine;

// A trigger collider that ends the game when the player falls into it.
// The world generator places it at some distance below the player
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
