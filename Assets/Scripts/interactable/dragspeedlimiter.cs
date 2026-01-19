using UnityEngine;

[RequireComponent(typeof(GooseDrag))]
public class DragSpeedLimiter : MonoBehaviour
{
    public float maxDragDistance = 1.5f;   // max allowed distance between player & object
    public float slowDownStrength = 5f;    // how quickly player speed adjusts when dragging

    public float normalSpeed = 6f;         // your player’s normal movement speed
    private float currentSpeed;

    private GooseDrag drag;
    private Rigidbody draggedRb;

    void Start()
    {
        drag = GetComponent<GooseDrag>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        draggedRb = drag ? drag.GetGrabbedRigidbody() : null;

        if (draggedRb == null)
        {
            // No drag = full speed
            currentSpeed = Mathf.Lerp(currentSpeed, normalSpeed, Time.deltaTime * slowDownStrength);
        }
        else
        {
            LimitMovement();
        }
    }

    void LimitMovement()
    {
        float dist = Vector3.Distance(transform.position, draggedRb.position);

        if (dist > maxDragDistance)
        {
            // If player is pulling too far, slow them down
            currentSpeed = Mathf.Lerp(
                currentSpeed,
                normalSpeed * 0.2f,                // heavily slowed speed
                Time.deltaTime * slowDownStrength
            );
        }
        else
        {
            // If within distance, speed adjusts toward normal-but-slightly-weighed speed
            currentSpeed = Mathf.Lerp(
                currentSpeed,
                normalSpeed * 0.7f,                // mild slowdown while dragging
                Time.deltaTime * slowDownStrength
            );
        }
    }

    // Public getter for other controllers
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
