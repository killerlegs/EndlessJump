using System.Collections.Generic;
using UnityEngine;

// Spawns chunk prefabs above the player and destroys them once they fall
// far enough below. Owns the lifecycle of every chunk it places, including
// the pre-placed starting chunks.
public class WorldGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] Transform mainCamera;
    [SerializeField] GameObject[] chunkPrefabs;
    Camera cameraComponent;

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
    [SerializeField] Material killZoneMaterial;
    [SerializeField] float killZoneLineWidth = 0.3f;

    float nextSpawnY;
    Transform killZone;
    readonly List<GameObject> spawnedChunks = new List<GameObject>();

    void Start()
    {
        nextSpawnY = transform.position.y + firstChunkOffset;
        spawnedChunks.AddRange(startingChunks);

        killZone = CreateKillZone();

        cameraComponent = mainCamera.GetComponent<Camera>();
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
        collider.size = new Vector2(killZoneWidth, killZoneLineWidth);

        killZoneObject.AddComponent<KillZone>();
        LineRenderer line = killZoneObject.AddComponent<LineRenderer>();

        line.material = killZoneMaterial;
        line.positionCount = 2;
        line.useWorldSpace = false;

        line.SetPosition(0, new Vector3(-killZoneWidth / 2f, 0f, 0f));
        line.SetPosition(1, new Vector3(killZoneWidth / 2f, 0f, 0f));

        line.startWidth = killZoneLineWidth;
        line.endWidth = killZoneLineWidth;

        line.startColor = Color.red;
        line.endColor = Color.red;

        line.sortingOrder = 20;
        return killZoneObject.transform;
    }
    void UpdateKillZone()
    {
        float bottomY =
            mainCamera.position.y - cameraComponent.orthographicSize;

        float newY = Mathf.Max(
            killZone.position.y,
            bottomY + 0.5f
        );

        killZone.position = new Vector3(
            killZone.position.x,
            newY,
            killZone.position.z
        );
    }
   void Update()
    {
        float highestPosition = Mathf.Max(
            player.position.y,
            mainCamera.position.y
        );

        if (nextSpawnY - highestPosition < spawnAheadDistance)
        {
            Spawn();
        }

        DespawnChunksBelowPlayer();

        UpdateKillZone();
    }

    // Instantiate a random chunk above the player.
    void Spawn()
    {
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        GameObject chunk = Instantiate(prefab, new Vector3(0f, nextSpawnY, 0f), Quaternion.identity);
        // Parent all chunks to this object, to keep the hierarchy clean.
        chunk.transform.SetParent(transform);
        spawnedChunks.Add(chunk);

        nextSpawnY += chunkHeight;
    }

    // Despawn chunks if their distance to the player is > despawn distance.
    void DespawnChunksBelowPlayer()
    {
        float highestPosition = Mathf.Max(
            player.position.y,
            mainCamera.position.y
        );

        for (int i = spawnedChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = spawnedChunks[i];

            if (highestPosition - chunk.transform.position.y > despawnDistance)
            {
                spawnedChunks.RemoveAt(i);
                Destroy(chunk);
            }
        }
    }
}
