using UnityEngine;

public class VehicleAI_LineOfSight : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Stats Set By EnemyManager")]
    public float moveSpeed = 8f;
    public float turnSpeed = 60f;
    public float visionRange = 25f;
    public float reactionDelay = 0.5f;

    [Header("Line Of Sight")]
    public LayerMask lineOfSightBlockingLayers;

    private Vector3 lastKnownPlayerPosition;
    private float nextReactionTime;

    private void Start()
    {
        if (player != null)
        {
            lastKnownPlayerPosition = player.position;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        UpdateTargetPosition();
        SteerTowardLastKnownPosition();
        MoveForward();
    }

    private void UpdateTargetPosition()
    {
        if (Time.time < nextReactionTime)
        {
            return;
        }

        nextReactionTime = Time.time + reactionDelay;

        if (CanSeePlayer())
        {
            lastKnownPlayerPosition = player.position;
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 directionToPlayer = player.position - origin;

        if (directionToPlayer.magnitude > visionRange)
        {
            return false;
        }

        if (Physics.Raycast(origin, directionToPlayer.normalized, out RaycastHit hit, visionRange, lineOfSightBlockingLayers))
        {
            return hit.transform == player || hit.transform.CompareTag("Player");
        }

        return false;
    }

    private void SteerTowardLastKnownPosition()
    {
        Vector3 direction = lastKnownPlayerPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.1f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

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