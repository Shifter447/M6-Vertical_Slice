using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    public float interactDistance = 2f;
    public Transform carryPoint;
    public LayerMask interactLayer;

    private Transform carriedObject;

    void Update()
    {
        // Right-click toggles pick up / drop
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
        // Prevent picking up while drag script is holding something
        GooseDrag drag = GetComponent<GooseDrag>();
        if (drag != null && drag.GetGrabbedRigidbody() != null)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance, interactLayer);
        if (hits.Length == 0) return;

        // Find nearest interactable object
        Collider nearest = hits[0];
        float bestDist = Vector3.Distance(transform.position, nearest.transform.position);

        foreach (var h in hits)
        {
            float d = Vector3.Distance(transform.position, h.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                nearest = h;
            }
        }

        carriedObject = nearest.transform;

        // Disable physics while carried
        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Snap into hand / beak position
        carriedObject.SetParent(carryPoint);
        carriedObject.localPosition = Vector3.zero;
        carriedObject.localRotation = Quaternion.identity;
    }

    void Drop()
    {
        if (carriedObject == null) return;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        carriedObject.SetParent(null);
        carriedObject = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
