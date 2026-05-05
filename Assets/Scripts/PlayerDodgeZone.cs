using Unity.VisualScripting;
using UnityEngine;

public class PlayerDodgeZone: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out VehicleCrashDestroy enemyBase))
        {
            enemyBase.wasInDodgeZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out VehicleCrashDestroy enemybase))
        {
            if (enemybase.wasInDodgeZone)
            {
                ScoringSystem.Instance.AddScore();
                Destroy(enemybase.gameObject);
            }
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
