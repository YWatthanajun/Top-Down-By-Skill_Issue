using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShield : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Vector3 spawnAreaSize;
    public int maxNumberOfPrefabsToSpawn;
    public float spawnCooldown = 15f; // Time between each spawn
    public int currentSpawnCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPrefabsRepeatedly());
    }

    IEnumerator SpawnPrefabsRepeatedly()
    {
        // Repeat the spawning process indefinitely
        while (true)
        {
            // Check if the current spawn count is less than the maximum allowed
            if (currentSpawnCount < maxNumberOfPrefabsToSpawn)
            {
                // Spawn the prefabs
                SpawnPrefabs();

                // Increment the spawn count
                currentSpawnCount++;
            }

            // Wait for spawn interval before spawning again
            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    void SpawnPrefabs()
    {
        // Calculate a random position within the spawn area
        Vector3 randomSpawnPosition = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            0f,
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        // Instantiate the prefab at the random position
        Instantiate(prefabToSpawn, randomSpawnPosition, Quaternion.identity);
    }
}
