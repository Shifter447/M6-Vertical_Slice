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

    private ContinuousInteractionAudio interactionAudio;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        interactionAudio = GetComponent<ContinuousInteractionAudio>();
    }

    void FixedUpdate()
    {
        if (!isBeingDragged || player == null)
            return;

        float distance = Vector3.Distance(rb.position, player.position);

        // Hard fail-safe (teleports, explosions, etc.)
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

        if (interactionAudio != null)
        {
            interactionAudio.StartLoop();
        }
    }

    public void StopDragging()
    {
        isBeingDragged = false;
        player = null;
        dragger = null;

        if (interactionAudio != null)
        {
            interactionAudio.StopLoop();
        }
    }

    void ForceRelease()
    {
        StopDragging();

        if (dragger != null)
        {
            dragger.Release();
        }
    }

    void OnDisable()
    {
        StopDragging();
    }

    public Rigidbody GetRigidbody() => rb;
}
