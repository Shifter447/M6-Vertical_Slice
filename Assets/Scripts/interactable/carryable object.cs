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
    public float pickupRadius = 1f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = carryWeight;

        if (pickupPoint == null)
            pickupPoint = transform;
    }

    public Vector3 GetPickupPosition()
    {
        return pickupPoint.position;
    }

    public void OnPickup(Transform carryPoint)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.detectCollisions = false;

        transform.SetParent(carryPoint);
        transform.localPosition = carryOffset;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDrop()
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.detectCollisions = true;
    }
}
