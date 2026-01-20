using UnityEngine;

public class FixedAngleCamera : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform player;

    [Header("Camera Offset")]
    // Base offset direction (will be normalized internally)
    public Vector3 offset = new Vector3(0, 10, -10);

    [Header("Zoom Settings")]
    public float zoomSpeed = 4f;
    public float minZoomDistance = 5f;
    public float maxZoomDistance = 20f;

    [Header("Smooth Follow")]
    public float smoothSpeed = 0.125f;

    private float currentZoomDistance;

    void Awake()
    {
        // Initialize zoom distance from offset magnitude
        currentZoomDistance = offset.magnitude;
        offset = offset.normalized;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        HandleZoom();

        Vector3 desiredPosition =
            player.position + offset * currentZoomDistance;

        Vector3 smoothedPosition =
            Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        // ORIGINAL BEHAVIOR: camera always looks at player
        transform.LookAt(player.position);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) < 0.001f)
            return;

        currentZoomDistance -= scroll * zoomSpeed;
        currentZoomDistance = Mathf.Clamp(
            currentZoomDistance,
            minZoomDistance,
            maxZoomDistance
        );
    }
}
