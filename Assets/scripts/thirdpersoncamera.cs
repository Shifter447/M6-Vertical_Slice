using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform player;         // Player to follow
    public Vector3 offset = new Vector3(0, 5, -10); // Default offset
    public float followSmoothSpeed = 0.125f;        // Camera position smoothing

    [Header("Zoom Settings")]
    public float zoomSpeed = 10f;    // Zoom interpolation speed
    public float minZoom = 30f;      // Min FOV
    public float maxZoom = 60f;      // Max FOV
    public float scrollMultiplier = 20f; // How much scroll affects zoom

    private Camera cam;
    private float targetFOV;
    private float zoomVelocity = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            cam = Camera.main;

        targetFOV = cam.fieldOfView;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // --- Smooth Follow ---
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSmoothSpeed);
        transform.position = smoothedPosition;

        // Optional: Rotate camera to look at the player smoothly
        transform.LookAt(player);

        // --- Mouse Scroll Zoom ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            targetFOV -= scroll * scrollMultiplier;
            targetFOV = Mathf.Clamp(targetFOV, minZoom, maxZoom);
        }

        // Smooth FOV change
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetFOV, ref zoomVelocity, 0.1f);
    }
}
