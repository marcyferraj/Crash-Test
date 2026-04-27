using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    /// <summary>
    /// Enemy Base Class
    /// 
    /// player - a reference to the player's transform. 
    ///     for simple implementation this can be a snapshot of the current position
    ///     for harder implementations this should be an actual player reference
    /// 
    /// updateInterval - a number of fixedUpdate calls. controls how often enemy samples player 
    ///     location and turns towards it
    /// </summary>

    [SerializeField]
    GameObject player;
    [SerializeField]
    int updateInterval = 10;
    [SerializeField]
    int forceMultiplier = 1;
    private int updateCounter;
    Rigidbody myBody;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myBody = GetComponent<Rigidbody>();
        updateCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(updateCounter % updateInterval == 0)
        {
            UnityEngine.Vector3 force = player.transform.position - transform.position;
            myBody.AddForce(force  * forceMultiplier);
            updateCounter = updateCounter % updateInterval;
        }
        updateCounter++;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WallTrigger"))
        {
            Destroy(gameObject);
            ScoringSystem.score += 1;
            
        }
    }
}
