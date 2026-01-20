using UnityEngine;

public class GooseDrag : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 2f;
    public Transform carryPoint;
    public LayerMask interactLayer;

    [Header("Base Spring Settings")]
    public float baseSpring = 180f;
    public float baseDamper = 12f;
    public float springSlack = 0.6f;

    [Header("Player Pushback")]
    public float playerPullForce = 40f;

    private SpringJoint joint;
    private Rigidbody grabbedRb;
    private DraggableObject draggable;
    private Rigidbody playerRb;

    private Collider[] playerColliders;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            TryGrab();

        if (Input.GetMouseButtonUp(1))
            Release();
    }

    void FixedUpdate()
    {
        if (joint == null)
            return;

        joint.connectedAnchor = carryPoint.position;
        ApplyPlayerResistance();
    }

    void TryGrab()
    {
        if (joint != null)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance, interactLayer);
        if (hits.Length == 0)
            return;

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

        draggable = nearest.GetComponent<DraggableObject>();
        if (draggable == null)
            return;

        grabbedRb = draggable.GetRigidbody();

        IgnorePlayerCollision(true);

        joint = grabbedRb.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = null;
        joint.connectedAnchor = carryPoint.position;

        float weight = draggable.dragWeight;
        joint.spring = baseSpring / weight;
        joint.damper = baseDamper;
        joint.maxDistance = springSlack;

        draggable.StartDragging(transform, this);
    }

    void ApplyPlayerResistance()
    {
        if (playerRb == null || draggable == null)
            return;

        float maxReach = draggable.maxDragDistance;
        Vector3 toObject = grabbedRb.position - transform.position;
        float distance = toObject.magnitude;

        if (distance > maxReach)
        {
            float excess = distance - maxReach;
            playerRb.AddForce(
                toObject.normalized * excess * playerPullForce,
                ForceMode.Acceleration
            );
        }
    }

    public void Release()
    {
        if (grabbedRb != null)
            IgnorePlayerCollision(false);

        if (joint != null)
            Destroy(joint);

        if (draggable != null)
            draggable.StopDragging();

        joint = null;
        grabbedRb = null;
        draggable = null;
    }

    void IgnorePlayerCollision(bool ignore)
    {
        if (grabbedRb == null)
            return;

        Collider[] objectColliders = grabbedRb.GetComponentsInChildren<Collider>();

        foreach (var pc in playerColliders)
        {
            foreach (var oc in objectColliders)
            {
                Physics.IgnoreCollision(pc, oc, ignore);
            }
        }
    }

    // ===== Compatibility Methods =====

    public Rigidbody GetGrabbedRigidbody()
    {
        return grabbedRb;
    }

    public float GetDraggedWeight()
    {
        return draggable != null ? draggable.dragWeight : 0f;
    }

    public bool IsDragging()
    {
        return joint != null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
