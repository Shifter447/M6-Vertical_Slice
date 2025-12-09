using UnityEngine;

public class CarryableObject : MonoBehaviour
{
    [Header("Carry Settings")]
    public float carryWeight = 1f;       // Optional, only if you want weight
    public Vector3 carryOffset;          // Optional offset while carried

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    // Called by GooseCarry when picked up
    public void OnPickup(Transform carryPoint)
    {
        rb.useGravity = false;
        rb.isKinematic = true;

        transform.SetParent(carryPoint);
        transform.localPosition = carryOffset;
        transform.localRotation = Quaternion.identity;
    }

    // Called by GooseCarry when dropped
    public void OnDrop()
    {
        transform.SetParent(null);

        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
