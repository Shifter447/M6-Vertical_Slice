using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject : MonoBehaviour
{
    [Header("Drag Weight (1 = light, 10+ = heavy)")]
    [Range(0.1f, 15f)]
    public float dragWeight = 1f;

    [Header("Drag Range")]
    public float maxDragDistance = 3f;

    private Rigidbody rb;
    private Transform player;
    private GooseDrag dragger;
    private bool isBeingDragged;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        if (!isBeingDragged || player == null) return;

        float distance = Vector3.Distance(rb.position, player.position);

        // Hard fail-safe only (teleports, explosions, etc.)
        if (distance > maxDragDistance * 1.5f)
        {
            ForceRelease();
        }
    }

    public void StartDragging(Transform playerTransform, GooseDrag source)
    {
        player = playerTransform;
        dragger = source;
        isBeingDragged = true;
    }

    public void StopDragging()
    {
        isBeingDragged = false;
        player = null;
        dragger = null;
    }

    void ForceRelease()
    {
        if (dragger != null)
            dragger.Release();
    }

    public Rigidbody GetRigidbody() => rb;
}
