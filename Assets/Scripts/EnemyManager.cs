using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Prefab and Spawn Points")]
    public GameObject vehiclePrefab;
    public Transform[] spawnPoints;

    [Header("Spawn Limits")]
    public int minimumVehicles = 10;
    public int maximumVehicles = 20;

    [Header("Periodic Spawning")]
    public float minimumSpawnTime = 10f;
    public float maximumSpawnTime = 20f;

    private List<GameObject> activeVehicles = new List<GameObject>();

    private void Start()
    {
        // Fill the level with the starting minimum amount of vehicles.
        SpawnUntilMinimumReached();

        // Start spawning extra vehicles every 10-20 seconds.
        StartCoroutine(PeriodicSpawn());
    }

    private void Update()
    {
        // Remove vehicles from the list if they were destroyed.
        activeVehicles.RemoveAll(vehicle => vehicle == null);

        // If the number drops below the minimum, refill it.
        SpawnUntilMinimumReached();
    }

    private void SpawnUntilMinimumReached()
    {
        while (activeVehicles.Count < minimumVehicles && activeVehicles.Count < maximumVehicles)
        {
            SpawnVehicle();
        }
    }

    private IEnumerator PeriodicSpawn()
    {
        while (true)
        {
            float waitTime = Random.Range(minimumSpawnTime, maximumSpawnTime);
            yield return new WaitForSeconds(waitTime);

            activeVehicles.RemoveAll(vehicle => vehicle == null);

            if (activeVehicles.Count < maximumVehicles)
            {
                SpawnVehicle();
            }
        }
    }

    private void SpawnVehicle()
    {
        if (vehiclePrefab == null)
        {
            Debug.LogWarning("Vehicle Prefab has not been assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points have been assigned.");
            return;
        }

        Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newVehicle = Instantiate(
            vehiclePrefab,
            chosenSpawnPoint.position,
            chosenSpawnPoint.rotation
        );

        activeVehicles.Add(newVehicle);
    }
}