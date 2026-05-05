using UnityEngine;

public class VehicleAI_TunnelVision : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Stats Set By EnemyManager")]
    public float moveSpeed = 8f;
    public float turnSpeed = 60f;

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        SteerTowardPlayer();
        MoveForward();
    }

    private void SteerTowardPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude < 0.1f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    private void MoveForward()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}