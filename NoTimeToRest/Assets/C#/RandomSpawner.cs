using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnRadius;
    public int numberOfPrefabsToSpawn;
    public float delayStart = 2f;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player GameObject
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Start the spawning coroutine
        Invoke("StartSpawn", delayStart);
    }

    void StartSpawn()
    {
        StartCoroutine(SpawnPrefabsRepeatedly());
    }

    IEnumerator SpawnPrefabsRepeatedly()
    {
        // Repeat the spawning process indefinitely
        while (true)
        {
            // Spawn the prefabs
            SpawnPrefabs();

            // Wait for three seconds before spawning again
            yield return new WaitForSeconds(3f);
        }
    }

    void SpawnPrefabs()
    {
        // Loop to spawn multiple prefabs
        for (int i = 0; i < numberOfPrefabsToSpawn; i++)
        {
            // Calculate a random position around the player within the spawn radius
            Vector3 randomSpawnOffset = Random.insideUnitSphere * spawnRadius;
            randomSpawnOffset.y = 0; // Ensure the prefab is spawned at the same height as the player
            Vector3 randomSpawnPosition = playerTransform.position + randomSpawnOffset;

            // Instantiate the prefab at the random position
            Instantiate(prefabToSpawn, randomSpawnPosition, Quaternion.identity);
        }
    }
}
