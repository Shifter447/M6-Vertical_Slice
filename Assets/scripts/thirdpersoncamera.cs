using UnityEngine;

public class FixedAngleCamera : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform player;

    [Header("Camera Offset")]
    // Elevated and back at a 45-degree angle
    public Vector3 offset = new Vector3(0, 10, -10);

    [Header("Smooth Follow")]
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (player == null) return;

        // Desired position: player position plus offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply smoothed position
        transform.position = smoothedPosition;

        // Make camera look at the player
        transform.LookAt(player.position);
    }
}
