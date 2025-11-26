using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GooseMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;   // Adjustable speed

    [Header("Input Setting")]
    [SerializeField] float sampleDistance = 0.5f;   // Max distance to find walkable point
    [SerializeField] LayerMask groundLayer;

    public static event System.Action<Vector3> OnGroundTouch;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed; // Set initial speed
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, groundLayer))
            {
                // Check if the clicked point is on or near the NavMesh
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, sampleDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(navMeshHit.position);

                    OnGroundTouch?.Invoke(navMeshHit.position);
                }
                else
                    Debug.Log("Clicked point is not a walkable area.");
            }
        }

        // Player Animation
        float normalizedSpeed = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
        anim.SetFloat("speed", normalizedSpeed);
    }
}