using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // Array to hold multiple prefabs
    public float spawnRadius;
    public int numberOfPrefabsToSpawn;
    public float delayStart = 2f;
    public float delaySpawn = 3f;
    public int position_x = 6;
    public int position_z = 2;
    public float minimumDistanceBetweenSpawns = 2f; // Minimum distance between each spawn


    private Transform playerTransform;
    private List<Vector3> spawnedPositions = new List<Vector3>();

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
            yield return new WaitForSeconds(delaySpawn);
        }
    }

    void SpawnPrefabs()
    {
        // Loop to spawn multiple prefabs
        for (int i = 0; i < numberOfPrefabsToSpawn; i++)
        {
            Vector3 randomSpawnPosition = Vector3.zero;
            bool isPositionValid = false;
            int attemptCounter = 0; // To prevent an infinite loop

            while (!isPositionValid && attemptCounter < 100) // Try up to 100 times for each spawn to find a valid position
            {
                Vector3 randomSpawnOffset = Random.insideUnitSphere * spawnRadius;
                randomSpawnOffset.y = 0; // Ensure the prefab spawns at ground level
                randomSpawnPosition = playerTransform.position + randomSpawnOffset;

                isPositionValid = true; // Assume the position is valid until proven otherwise


                // Check if the position is outside the restricted area
                if (Mathf.Abs(randomSpawnPosition.x - playerTransform.position.x) <= position_x && Mathf.Abs(randomSpawnPosition.z - playerTransform.position.z) <= position_z)
                {
                    isPositionValid = false; // Too close to the player
                }

                // Check against all other spawned positions
                foreach (Vector3 otherPosition in spawnedPositions)
                {
                    if (Vector3.Distance(randomSpawnPosition, otherPosition) < minimumDistanceBetweenSpawns)
                    {
                        isPositionValid = false; // Too close to another spawn
                        break; // No need to check further
                    }
                }

                attemptCounter++;
            }

            if (isPositionValid)
            {
                // Randomly select one of the prefabs to spawn
                GameObject selectedPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
                Instantiate(selectedPrefab, randomSpawnPosition, Quaternion.identity);
                spawnedPositions.Add(randomSpawnPosition); // Keep track of the spawn position
            }
            else
            {
                Debug.LogWarning("Could not find a valid position for new spawn after 100 attempts.");
            }
        }
    }
}
