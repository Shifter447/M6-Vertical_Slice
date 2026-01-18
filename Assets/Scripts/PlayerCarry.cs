using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 3f; // distance player can pick up objects
    public Transform carryPoint; // assign in inspector
    public LayerMask interactLayer; // must include layer of CarryableObjects

    private CarryableObject carriedObject;

    void Update()
    {
        // Only right-click
        if (Input.GetMouseButtonDown(1))
        {
            if (carriedObject == null)
                TryPickUp();
            else
                Drop();
        }
    }

    void TryPickUp()
    {
        // Find all colliders within interactDistance on interactLayer
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance, interactLayer);

        if (hits.Length == 0)
            return;

        CarryableObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            CarryableObject carryable = hit.GetComponent<CarryableObject>();
            if (carryable == null)
                continue;

            float dist = Vector3.Distance(transform.position, carryable.GetPickupPosition());

            if (!carryable.CanPickupFrom(transform.position))
                continue;

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = carryable;
            }
        }

        if (closest != null)
        {
            carriedObject = closest;
            carriedObject.OnPickup(carryPoint);
        }
    }

    void Drop()
    {
        if (carriedObject == null)
            return;

        carriedObject.OnDrop();
        carriedObject = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
