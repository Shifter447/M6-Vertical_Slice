using UnityEngine;

public class PlayerInteractIndicator : MonoBehaviour
{
    [Header("Detection")]
    public float interactRange = 2f;
    public LayerMask interactLayer;

    [Header("Animation")]
    public Animator animator;
    public string inRangeBool = "InInteractRange";

    private bool isInRange;

    void Update()
    {
        bool foundInteractable = CheckForInteractable();

        if (foundInteractable != isInRange)
        {
            isInRange = foundInteractable;
            animator.SetBool(inRangeBool, isInRange);
        }
    }

    bool CheckForInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            interactRange,
            interactLayer
        );

        foreach (var hit in hits)
        {
            if (hit.GetComponent<DraggableObject>() != null)
                return true;

            if (hit.GetComponent<CarryableObject>() != null)
                return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
