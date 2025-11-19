using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] waypoints;   // Set these in the Inspector
    public float stoppingDistance = 0.2f;

    private int currentWaypointIndex = 0;

    public Transform GetCurrentWaypoint()
    {
        if (waypoints.Length == 0) return null;
        return waypoints[currentWaypointIndex];
    }

    public bool ReachedFinalWaypoint()
    {
        return currentWaypointIndex >= waypoints.Length - 1;
    }

    public void AdvanceToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            currentWaypointIndex++;
        }
    }

    public bool IsAtWaypoint(Transform waypoint)
    {
        return Vector3.Distance(transform.position, waypoint.position) < stoppingDistance;
    }
}
