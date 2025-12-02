using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Player to follow
    public Vector3 offset;         // Camera offset from player
    public float smoothSpeed = 0.125f; // How smoothly the camera follows

    void LateUpdate()
    {
        if (player == null) return;

        // Desired camera position
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Look at the player
        transform.LookAt(player);
    }
}
