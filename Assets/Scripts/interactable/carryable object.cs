using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class CarryableObject : MonoBehaviour
{
    [Header("Carry Settings")]
    public float carryWeight = 1f;
    public Vector3 carryOffset = Vector3.zero;

    [Header("Pickup Point")]
    public Transform pickupPoint;
    public float pickupRadius = 1f; // increased for easier pickup

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = carryWeight;

        // Safety
        if (pickupPoint == null)
            pickupPoint = transform;
    }

    // ===== PICKUP VALIDATION =====
    public bool CanPickupFrom(Vector3 playerPosition)
    {
        return Vector3.Distance(playerPosition, GetPickupPosition()) <= pickupRadius;
    }

    public Vector3 GetPickupPosition()
    {
        return pickupPoint != null ? pickupPoint.position : transform.position;
    }

    // ===== PICKUP & DROP =====
    public void OnPickup(Transform carryPoint)
    {
        rb.useGravity = false;
        rb.isKinematic = true;

        transform.SetParent(carryPoint);
        transform.localPosition = carryOffset;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDrop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    // ===== DEBUG =====
    void OnDrawGizmosSelected()
    {
        if (pickupPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pickupPoint.position, pickupRadius);
    }
}
