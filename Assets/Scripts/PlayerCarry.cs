using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    public float interactDistance = 2f;
    public Transform carryPoint;            // assign in Inspector
    public LayerMask interactLayer;

    private Transform carriedObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TryPickUp();
        }

        if (Input.GetMouseButtonUp(1))
        {
            Drop();
        }
    }

    void TryPickUp()
    {
        if (carriedObject != null) return;

        // Check for nearby objects
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance, interactLayer);

        if (hits.Length == 0) return;

        // Pick the closest object
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

        // Parent to pivot
        carriedObject.SetParent(carryPoint);
        carriedObject.localPosition = Vector3.zero;
        carriedObject.localRotation = Quaternion.identity;
    }

    void Drop()
    {
        if (carriedObject == null) return;

        // Re-enable physics
        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // Unparent
        carriedObject.SetParent(null);
        carriedObject = null;
    }

    // Visualize distance in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
