using System.Collections.Generic;
using UnityEngine;

// Spawns chunk prefabs ahead of the player and destroys them once they fall
// far enough behind. Owns the lifecycle of every chunk it places, including
// the pre-placed starting chunks.
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
    [Tooltip("Pre-placed chunks at the level start; despawned like any other chunk.")]
    [SerializeField] GameObject[] startingChunks;
    [Tooltip("Offset so the first spawned chunk doesn't overlap the starting chunks.")]
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
        // Parent all chunks to this object, to keep the hierarchy clean.
        chunk.transform.SetParent(transform); 
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
