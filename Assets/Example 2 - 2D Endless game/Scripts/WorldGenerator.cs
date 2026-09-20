using System.Collections.Generic;
using UnityEngine;

// Spawns chunk prefabs above the player and destroys them once they fall
// far enough below. Owns the lifecycle of every chunk it places, including
// the pre-placed starting chunks.
public class WorldGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] GameObject[] chunkPrefabs;

    [Header("Spawning")]
    [SerializeField] float chunkHeight = 12f; 
    [SerializeField] float spawnAheadDistance = 24f;
    [SerializeField] float despawnDistance = 24f;

    [Header("Starting Chunks")]
    [Tooltip("Pre-placed chunks at the level start; despawned like any other chunk.")]
    [SerializeField] GameObject[] startingChunks;
    [Tooltip("Offset so the first spawned chunk doesn't overlap the starting chunks.")]
    [SerializeField] float firstChunkOffset = 12f;

    [Header("Kill Zone")]
    [Tooltip("Width of the kill zone that trails below the lowest loaded chunk.")]
    [SerializeField] float killZoneWidth = 50f;

    float nextSpawnY;
    Transform killZone;
    readonly List<GameObject> spawnedChunks = new List<GameObject>();

    void Start()
    {
        nextSpawnY = transform.position.y + firstChunkOffset;
        spawnedChunks.AddRange(startingChunks);
        killZone = CreateKillZone();
    }

    // A trigger that starts below the starting chunks and, as chunks despawn,
    // slides up to take the place of the last one destroyed - so falling
    // below the currently loaded chunks always means falling into it.
    Transform CreateKillZone()
    {
        GameObject killZoneObject = new GameObject("KillZone");
        killZoneObject.transform.SetParent(transform);
        killZoneObject.transform.position = new Vector3(0f, transform.position.y - despawnDistance, 0f);

        BoxCollider2D collider = killZoneObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(killZoneWidth, 1f);

        killZoneObject.AddComponent<KillZone>();
        return killZoneObject.transform;
    }

    void Update()
    {
        if (nextSpawnY - player.position.y < spawnAheadDistance)
        {
            Spawn();
        }

        DespawnChunksBelowPlayer();
    }

    void Spawn()
    {
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        GameObject chunk = Instantiate(prefab, new Vector3(0f, nextSpawnY, 0f), Quaternion.identity);
        // Parent all chunks to this object, to keep the hierarchy clean.
        chunk.transform.SetParent(transform);
        spawnedChunks.Add(chunk);

        nextSpawnY += chunkHeight;
    }

    void DespawnChunksBelowPlayer()
    {
        for (int i = spawnedChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = spawnedChunks[i];

            if (player.position.y - chunk.transform.position.y > despawnDistance)
            {
                spawnedChunks.RemoveAt(i);

                if (chunk.transform.position.y > killZone.position.y)
                {
                    killZone.position = new Vector3(killZone.position.x, chunk.transform.position.y, killZone.position.z);
                }

                Destroy(chunk);
            }
        }
    }
}
