using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;

    private EnemyPathing pathing;

    void Start()
    {
        pathing = GetComponent<EnemyPathing>();
    }

    void Update()
    {
        Transform target = pathing.GetCurrentWaypoint();
        if (target == null) return;

        // Move toward the current waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // When arriving at a waypoint
        if (pathing.IsAtWaypoint(target))
        {
            if (pathing.ReachedFinalWaypoint())
            {
                // Despawn enemy
                Destroy(gameObject);
            }
            else
            {
                // Move to the next waypoint
                pathing.AdvanceToNextWaypoint();
            }
        }
    }
}
