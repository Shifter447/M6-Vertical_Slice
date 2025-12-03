using UnityEngine;
using UnityEngine.AI;

public class GooseMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private GooseDrag drag;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;

    [Header("Input Setting")]
    [SerializeField] float sampleDistance = 0.5f;
    [SerializeField] LayerMask groundLayer;

    public static event System.Action<Vector3> OnGroundTouch;

    private float baseSpeed;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        drag = GetComponent<GooseDrag>();

        baseSpeed = moveSpeed;
        agent.speed = baseSpeed;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, groundLayer))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, sampleDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(navMeshHit.position);
                    OnGroundTouch?.Invoke(navMeshHit.position);
                }
            }
        }

        // Apply slowdown based on object weight
        float weight = drag ? drag.GetDraggedWeight() : 0f;

        if (weight > 0f)
        {
            float factor = 1f / (1f + weight * 0.4f);   // Simple weight curve
            float targetSpeed = baseSpeed * factor;

            agent.speed = Mathf.Lerp(agent.speed, targetSpeed, Time.deltaTime * 5f);
        }
        else
        {
            agent.speed = Mathf.Lerp(agent.speed, baseSpeed, Time.deltaTime * 5f);
        }

        float normalizedSpeed = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
        anim.SetFloat("speed", normalizedSpeed);
    }
}
