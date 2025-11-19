using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Follow Settings")]
    public float smoothSpeed = 5f; 
    public Vector3 offset = new Vector3(0, 5, -10);

    [Header("Optional Bounds")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + offset;

        // Smooth follow (damped)
        Vector3 smoothedPos = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            1f / smoothSpeed
        );

        // Optional clamp
        if (useBounds)
        {
            smoothedPos.x = Mathf.Clamp(smoothedPos.x, minBounds.x, maxBounds.x);
            smoothedPos.z = Mathf.Clamp(smoothedPos.z, minBounds.y, maxBounds.y);
        }

        transform.position = smoothedPos;
    }
}
