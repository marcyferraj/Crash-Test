using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Vehicle Setup")]
    public GameObject vehiclePrefab;
    public Transform player;
    public Transform[] spawnPoints;

    [Header("Spawn Limits")]
    public int startingMinimumVehicles = 10;
    public int startingMaximumVehicles = 20;

    public int maxDifficultyMinimumVehicles = 18;
    public int maxDifficultyMaximumVehicles = 35;

    [Header("Spawn Timing")]
    public float minimumSpawnTime = 10f;
    public float maximumSpawnTime = 20f;

    [Header("Difficulty Timing")]
    public float timeToReachMaxDifficulty = 300f; // 5 minutes

    [Header("Beginner Vehicle Stats")]
    public float beginnerSpeed = 8f;
    public float beginnerTurnSpeed = 60f;
    public float beginnerVisionRange = 25f;
    public float beginnerReactionDelay = 0.6f;

    [Header("Hard Vehicle Stats")]
    public float hardSpeed = 16f;
    public float hardTurnSpeed = 120f;
    public float hardVisionRange = 50f;
    public float hardReactionDelay = 0.1f;

    private float runStartTime;
    private List<GameObject> activeVehicles = new List<GameObject>();

    private void Start()
    {
        runStartTime = Time.time;

        SpawnUntilMinimumReached();
        StartCoroutine(PeriodicSpawnRoutine());
    }

    private void Update()
    {
        CleanVehicleList();
        SpawnUntilMinimumReached();
    }

    private IEnumerator PeriodicSpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minimumSpawnTime, maximumSpawnTime);
            yield return new WaitForSeconds(waitTime);

            CleanVehicleList();

            if (activeVehicles.Count < GetCurrentMaximumVehicles())
            {
                SpawnVehicle();
            }
        }
    }

    private void SpawnUntilMinimumReached()
    {
        while (activeVehicles.Count < GetCurrentMinimumVehicles() &&
               activeVehicles.Count < GetCurrentMaximumVehicles())
        {
            SpawnVehicle();
        }
    }

    private void SpawnVehicle()
    {
        if (vehiclePrefab == null)
        {
            Debug.LogWarning("EnemyManager: Vehicle Prefab is not assigned.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("EnemyManager: Player is not assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemyManager: No spawn points are assigned.");
            return;
        }

        Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newVehicle = Instantiate(
            vehiclePrefab,
            chosenSpawnPoint.position,
            chosenSpawnPoint.rotation
        );

        ScoringSystem scoringSystem = player.GetComponent<ScoringSystem>();

        if (scoringSystem == null)
        {
            Debug.LogError("Player is missing Scoring System");
        }
        else
        {
            VehicleCrashDestroy crash = newVehicle.GetComponent<VehicleCrashDestroy>();
            if (crash != null)
            {
                crash.initScoring(scoringSystem);
            }
        }
        ApplyDifficultyToVehicle(newVehicle);
        activeVehicles.Add(newVehicle);
    }

    private void ApplyDifficultyToVehicle(GameObject vehicle)
    {
        float difficulty = GetCurrentDifficulty();

        float currentSpeed = Mathf.Lerp(beginnerSpeed, hardSpeed, difficulty);
        float currentTurnSpeed = Mathf.Lerp(beginnerTurnSpeed, hardTurnSpeed, difficulty);
        float currentVisionRange = Mathf.Lerp(beginnerVisionRange, hardVisionRange, difficulty);
        float currentReactionDelay = Mathf.Lerp(beginnerReactionDelay, hardReactionDelay, difficulty);

        VehicleAI_TunnelVision tunnelVisionAI = vehicle.GetComponent<VehicleAI_TunnelVision>();

        if (tunnelVisionAI != null)
        {
            tunnelVisionAI.player = player;
            tunnelVisionAI.moveSpeed = currentSpeed;
            tunnelVisionAI.turnSpeed = currentTurnSpeed;
        }

        VehicleAI_LineOfSight lineOfSightAI = vehicle.GetComponent<VehicleAI_LineOfSight>();

        if (lineOfSightAI != null)
        {
            lineOfSightAI.player = player;
            lineOfSightAI.moveSpeed = currentSpeed;
            lineOfSightAI.turnSpeed = currentTurnSpeed;
            lineOfSightAI.visionRange = currentVisionRange;
            lineOfSightAI.reactionDelay = currentReactionDelay;
        }
    }

    private float GetCurrentDifficulty()
    {
        float runTime = Time.time - runStartTime;

        return Mathf.Clamp01(runTime / timeToReachMaxDifficulty);
    }

    private int GetCurrentMinimumVehicles()
    {
        float difficulty = GetCurrentDifficulty();

        return Mathf.RoundToInt(Mathf.Lerp(
            startingMinimumVehicles,
            maxDifficultyMinimumVehicles,
            difficulty
        ));
    }

    private int GetCurrentMaximumVehicles()
    {
        float difficulty = GetCurrentDifficulty();

        return Mathf.RoundToInt(Mathf.Lerp(
            startingMaximumVehicles,
            maxDifficultyMaximumVehicles,
            difficulty
        ));
    }

    private void CleanVehicleList()
    {
        activeVehicles.RemoveAll(vehicle => vehicle == null);
    }
}