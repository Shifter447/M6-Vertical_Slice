using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject : MonoBehaviour
{
    [Header("Drag Weight (1 = light, 10+ = heavy)")]
    [Range(0.1f, 15f)]
    public float dragWeight = 1f;

    [Header("Drag Range Settings")]
    public Transform player;              // Player reference
    public float maxDragDistance = 3f;     // Maximum allowed distance from player

    private Rigidbody rb;
    private bool isBeingDragged;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        if (!isBeingDragged || player == null)
            return;

        // Vector from player to object
        Vector3 offset = rb.position - player.position;

        // If object exceeds max distance, clamp it
        if (offset.magnitude > maxDragDistance)
        {
            Vector3 clampedPosition =
                player.position + offset.normalized * maxDragDistance;

            rb.MovePosition(clampedPosition);
        }
    }

    // Call this when dragging starts
    public void StartDragging()
    {
        isBeingDragged = true;
    }

    // Call this when dragging ends
    public void StopDragging()
    {
        isBeingDragged = false;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
