using UnityEngine;
using UnityEngine.Events;

public class ClickInteractable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 2f;
    public Transform player;

    [Tooltip("Leave empty to allow clicking on any layer.")]
    public LayerMask clickLayers = ~0;

    public UnityEvent onInteract;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryInteract();
    }

    void TryInteract()
    {
        if (player == null)
        {
            Debug.LogWarning($"{name}: No player assigned!");
            return;
        }

        // 1. Range check
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist > interactRange)
        {
            Debug.Log($"{name}: Player too far ({dist:F1} > {interactRange})");
            return;
        }

        // 2. Raycast from camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 200f, clickLayers))
        {
            Debug.Log($"{name}: Raycast hit nothing.");
            return;
        }

        // 3. Ensure the clicked object is THIS one
        if (hit.collider.gameObject != gameObject)
        {
            Debug.Log($"{name}: Raycast hit {hit.collider.name}, not this object.");
            return;
        }

        Debug.Log($"{name}: Interacted!");
        onInteract?.Invoke();
    }

    // Gizmo to visualize interaction range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
