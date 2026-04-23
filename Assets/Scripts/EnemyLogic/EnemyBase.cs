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
    int updateInterval;
    private int updateCounter = 10;
    Rigidbody myBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myBody = FindAnyObjectByType<Rigidbody>();
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
           myBody.AddForce(player.transform.position - transform.position);
           updateCounter = updateCounter % updateInterval;
        }
        updateCounter++;
        
    }
}
