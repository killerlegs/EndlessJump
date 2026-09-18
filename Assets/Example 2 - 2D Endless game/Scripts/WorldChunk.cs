using UnityEngine;

// Marker for a level chunk (ground + obstacles) placed by ChunkSpawner.
// Chunks are static - the player moves, not the level - and no longer
// despawn themselves; ChunkSpawner tracks and destroys them once they're
// far enough behind the player (see ChunkSpawner.DespawnChunksBehindPlayer).
public class WorldChunk : MonoBehaviour
{
}
