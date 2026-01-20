using UnityEngine;
using UnityEngine.AI;

public class GooseMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private GooseDrag drag;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float runMultiplier = 1.7f;

    [Header("Input Settings")]
    [SerializeField] float sampleDistance = 0.5f;
    [SerializeField] LayerMask groundLayer;

    public static event System.Action<Vector3> OnGroundTouch;

    private float baseSpeed;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        drag = GetComponent<GooseDrag>();
    }

    void Start()
    {
        baseSpeed = moveSpeed;
        agent.speed = baseSpeed;

        // IMPORTANT: allow NavMeshAgent to control rotation normally
        agent.updateRotation = true;
    }

    void Update()
    {
        HandleMovementInput();
        UpdateSpeed();
        UpdateAnimation();
    }

    void HandleMovementInput()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            if (NavMesh.SamplePosition(
                hit.point,
                out NavMeshHit navHit,
                sampleDistance,
                NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
                OnGroundTouch?.Invoke(navHit.position);
            }
        }
    }

    void UpdateSpeed()
    {
        bool isRunning = Input.GetMouseButton(0);
        float targetSpeed = isRunning
            ? baseSpeed * runMultiplier
            : baseSpeed;

        float weight = drag != null ? drag.GetDraggedWeight() : 0f;

        if (weight > 0f)
        {
            float slowdownFactor = 1f / (1f + weight * 0.4f);
            targetSpeed *= slowdownFactor;
        }

        agent.speed = Mathf.Lerp(
            agent.speed,
            targetSpeed,
            Time.deltaTime * 5f
        );
    }

    void UpdateAnimation()
    {
        float normalizedSpeed =
            Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);

        anim.SetFloat("speed", normalizedSpeed);
    }
}
