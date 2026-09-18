using System.Collections.Generic;
using UnityEngine;

// Streams the level ahead of the player by tiling random chunk prefabs
// edge-to-edge, spawning the next one once the player gets within
// spawnAheadDistance of the end of the last one. Chunks are static level
// geometry (see WorldChunk.cs) - this manager tracks every chunk it spawns
// and destroys the ones that have fallen far enough behind the player.
public class ChunkSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] GameObject[] chunkPrefabs;

    [Header("Spawning")]
    [SerializeField] float chunkWidth = 12f;
    [SerializeField] float spawnAheadDistance = 24f;
    [SerializeField] float despawnDistance = 24f;

    [Header("Starting Chunks")]
    [Tooltip("Pre-placed chunks at the start of the level (e.g. the starting ground). " +
        "Tracked for despawning just like spawned chunks, once the player has run far enough past them.")]
    [SerializeField] GameObject[] startingChunks;
    [Tooltip("Extra distance to the right of this spawner's position before the first spawned chunk is " +
        "placed, so it can't overlap the starting chunks.")]
    [SerializeField] float firstChunkOffset = 12f;

    float nextSpawnX;
    readonly List<GameObject> spawnedChunks = new List<GameObject>();

    void Start()
    {
        nextSpawnX = transform.position.x + firstChunkOffset;
        spawnedChunks.AddRange(startingChunks);
    }

    void Update()
    {
        if (nextSpawnX - player.position.x < spawnAheadDistance)
        {
            Spawn();
        }

        DespawnChunksBehindPlayer();
    }

    void Spawn()
    {
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        GameObject chunk = Instantiate(prefab, new Vector3(nextSpawnX, 0f, 0f), Quaternion.identity);
        chunk.transform.SetParent(transform); // Parent all chunks to this object, to keep the hierarchy clean.
        spawnedChunks.Add(chunk);

        nextSpawnX += chunkWidth;
    }

    void DespawnChunksBehindPlayer()
    {
        for (int i = spawnedChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = spawnedChunks[i];

            if (player.position.x - chunk.transform.position.x > despawnDistance)
            {
                spawnedChunks.RemoveAt(i);
                Destroy(chunk);
            }
        }
    }
}
