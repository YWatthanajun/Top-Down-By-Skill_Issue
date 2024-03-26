using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // Array to hold multiple prefabs
    public float spawnRadius;
    public int numberOfPrefabsToSpawn = 1;
    public float delayStart = 2f;
    public float delaySpawn = 1f;
    public float delayBetweenTurrets = 0.2f;
    public int position_x = 6;
    public int position_z = 2;
    public float minimumDistanceBetweenSpawns = 2f; // Minimum distance between each spawn
    public GameObject alertIndicatorPrefab;
    public float alertIndicatorDuration = 3f;


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
    void Update()
    {
        GameObject playerObject = GameObject.Find("Player");
        PlayerController player = playerObject.GetComponent<PlayerController>();
        if (player.percentCoin >= 5 && player.percentCoin <= 10)
        {
            numberOfPrefabsToSpawn = 3;
            delaySpawn = 1.5f;
        }
        else if (player.percentCoin >= 11 && player.percentCoin <= 25)
        {
            numberOfPrefabsToSpawn = 5;
            delaySpawn = 2f;
        }
        else if (player.percentCoin >= 26 && player.percentCoin <= 50)
        {
            numberOfPrefabsToSpawn = 7;
            delaySpawn = 2.5f;
        }
        else if (player.percentCoin >= 51 && player.percentCoin <= 75)
        {
            numberOfPrefabsToSpawn = 9;
            delaySpawn = 3f;
        }
        else if (player.percentCoin >= 76 && player.percentCoin <= 90)
        {
            numberOfPrefabsToSpawn = 11;
            delaySpawn = 2.5f;
        }
        else if (player.percentCoin >= 91 && player.percentCoin <= 100)
        {
            numberOfPrefabsToSpawn = 15;
            delaySpawn = 1f;
            delayBetweenTurrets = 0.1f;
        }
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
            StartCoroutine(SpawnPrefabs());

            // Wait for three seconds before spawning again
            yield return new WaitForSeconds(delaySpawn);
        }
    }

    IEnumerator SpawnPrefabs()
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
                // Spawn the alert indicator
                GameObject alertIndicator = Instantiate(alertIndicatorPrefab, randomSpawnPosition, Quaternion.identity);
                alertIndicator.SetActive(true);

                // Wait for the alert indicator duration
                yield return new WaitForSeconds(alertIndicatorDuration);

                // Randomly select one of the prefabs to spawn
                GameObject selectedPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
                Instantiate(selectedPrefab, randomSpawnPosition, Quaternion.identity);
                spawnedPositions.Add(randomSpawnPosition); // Keep track of the spawn position

                // Disable the alert indicator
                alertIndicator.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Could not find a valid position for new spawn after 100 attempts.");
            }

            yield return new WaitForSeconds(delayBetweenTurrets);
        }
    }
}
