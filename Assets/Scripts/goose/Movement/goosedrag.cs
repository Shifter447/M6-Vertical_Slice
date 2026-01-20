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

    // Cache player colliders
    private Collider[] playerColliders;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) TryGrab();
        if (Input.GetMouseButtonUp(1)) Release();
    }

    void FixedUpdate()
    {
        if (joint == null) return;

        joint.connectedAnchor = carryPoint.position;
        ApplyPlayerResistance();
    }

    void TryGrab()
    {
        if (joint != null) return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            interactDistance,
            interactLayer,
            QueryTriggerInteraction.Ignore
        );

        float bestDist = float.MaxValue;
        DraggableObject bestDraggable = null;

        foreach (var hit in hits)
        {
            // Ignore player colliders
            if (IsPlayerCollider(hit))
                continue;

            DraggableObject d = hit.GetComponent<DraggableObject>();
            if (d == null)
                continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestDraggable = d;
            }
        }

        if (bestDraggable == null)
            return;

        draggable = bestDraggable;
        grabbedRb = draggable.GetRigidbody();

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

    bool IsPlayerCollider(Collider col)
    {
        foreach (var pc in playerColliders)
        {
            if (col == pc)
                return true;
        }
        return false;
    }

    void ApplyPlayerResistance()
    {
        if (playerRb == null || draggable == null) return;

        float maxReach = draggable.maxDragDistance;
        Vector3 toObject = grabbedRb.position - transform.position;
        float distance = toObject.magnitude;

        if (distance > maxReach)
        {
            Vector3 pullDir = toObject.normalized;
            float excess = distance - maxReach;

            playerRb.AddForce(
                pullDir * excess * playerPullForce,
                ForceMode.Acceleration
            );
        }
    }

    public void Release()
    {
        if (joint != null) Destroy(joint);
        if (draggable != null) draggable.StopDragging();

        joint = null;
        grabbedRb = null;
        draggable = null;
    }

    public Rigidbody GetGrabbedRigidbody() => grabbedRb;
    public float GetDraggedWeight() => draggable ? draggable.dragWeight : 0f;
    public bool IsDragging() => joint != null;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
