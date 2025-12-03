using UnityEngine;

public class GooseDrag : MonoBehaviour
{
    public float interactDistance = 2f;
    public Transform carryPoint;
    public LayerMask interactLayer;

    public float spring = 250f;
    public float damper = 8f;
    public float maxDistance = 0.1f;

    private SpringJoint currentJoint;
    private Rigidbody grabbedRb;
    private DraggableObject draggable;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            TryGrab();

        if (Input.GetMouseButtonUp(1))
            Release();
    }

    void TryGrab()
    {
        if (currentJoint != null) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance, interactLayer);
        if (hits.Length == 0) return;

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
        if (draggable == null) return;

        grabbedRb = draggable.GetRigidbody();

        currentJoint = grabbedRb.gameObject.AddComponent<SpringJoint>();
        currentJoint.autoConfigureConnectedAnchor = false;
        currentJoint.connectedBody = null;
        currentJoint.connectedAnchor = carryPoint.position;
        currentJoint.spring = spring;
        currentJoint.damper = damper;
        currentJoint.maxDistance = maxDistance;
    }

    void FixedUpdate()
    {
        if (currentJoint != null)
            currentJoint.connectedAnchor = carryPoint.position;
    }

    void Release()
    {
        if (currentJoint != null)
        {
            Destroy(currentJoint);
            currentJoint = null;
        }

        grabbedRb = null;
        draggable = null;
    }

    public float GetDraggedWeight()
    {
        return draggable ? draggable.dragWeight : 0f;
    }

    public Rigidbody GetGrabbedRigidbody()
    {
        return grabbedRb;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
