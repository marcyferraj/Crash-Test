using System;
using UnityEngine;

public class VehicleCrashDestroy : MonoBehaviour
{
    [Header("Spawn Protection")]
    public float spawnInvincibilityTime = 1.5f;

    private float spawnTime;
    public ScoringSystem scoreSystem;

    private void Start()
    {
        spawnTime = Time.time;        
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool spawnProtected = Time.time - spawnTime < spawnInvincibilityTime;

        if (scoreSystem == null)
        {
            Debug.LogWarning("ScoringSystem Not Assigned");
        }

        if (collision.gameObject.CompareTag("Obstacle") ||
            collision.transform.root.CompareTag("Obstacle"))
        {
            
            Destroy(gameObject);
            scoreSystem.AddScore();
            return;
        }

        if (collision.gameObject.CompareTag("Player") ||
            collision.transform.root.CompareTag("Player"))
        {
            PlayerLife playerLife = collision.transform.root.GetComponent<PlayerLife>();

            if (playerLife != null)
            {
                scoreSystem.MinusScore();
                playerLife.Die();
            }

            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Vehicle") ||
            collision.transform.root.CompareTag("Vehicle"))
        {
            if (spawnProtected)
            {
                return;
            }
            scoreSystem.AddScore();
            Destroy(gameObject);
            return;
        }
    }

    public void initScoring(ScoringSystem scoringSystem)
    {
        scoreSystem = scoringSystem;
    }
}