using UnityEngine;
using UnityEngine.AI;

public class CameraFollowAnchor : MonoBehaviour
{
    public Transform anchor;

    [Header("Stability")]
    public float smoothTime = 0.12f;

    private NavMeshAgent agent;
    private Vector3 velocity;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void LateUpdate()
    {
        if (anchor == null || agent == null)
            return;

        Vector3 targetPosition = agent.nextPosition;

        anchor.position = Vector3.SmoothDamp(
            anchor.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}
