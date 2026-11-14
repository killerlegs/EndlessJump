using UnityEngine;

// Tags a collider as deadly. Player checks for this component on whatever
// it collides with (see Player.OnCollisionEnter) instead of using tags or
// layers - a component reference can't be misspelled the way a tag string can.
public class KillZone : MonoBehaviour
{
}
