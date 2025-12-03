using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject : MonoBehaviour
{
    [Header("Drag Weight (1 = light, 10+ = heavy)")]
    [Range(0.1f, 15f)]
    public float dragWeight = 1f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
