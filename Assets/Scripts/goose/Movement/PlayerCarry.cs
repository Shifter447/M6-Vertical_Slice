using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 3f;
    public Transform carryPoint;
    public LayerMask interactLayer;

    private CarryableObject carriedObject;

    void Update()
    {
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
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            interactDistance,
            interactLayer
        );

        CarryableObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            CarryableObject carryable = hit.GetComponentInParent<CarryableObject>();
            if (carryable == null)
                continue;

            float dist = Vector3.Distance(
                transform.position,
                carryable.GetPickupPosition()
            );

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
        carriedObject.OnDrop();
        carriedObject = null;
    }
}
