using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))] // Optional: ensures audio source exists
public class CarryableObject : MonoBehaviour
{
    [Header("Carry Settings")]
    public float carryWeight = 1f;
    public Vector3 carryOffset = Vector3.zero;

    [Header("Pickup Point")]
    public Transform pickupPoint;
    public float pickupRadius = 1f;

    [Header("Audio")]
    public AudioClip carryClip; // Sound to play while carrying

    private Rigidbody rb;
    private AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = carryWeight;

        if (pickupPoint == null)
            pickupPoint = transform;

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = true;
        }
    }

    /// <summary>
    /// Returns the position where the player should pick up the object.
    /// </summary>
    public Vector3 GetPickupPosition()
    {
        return pickupPoint.position;
    }

    /// <summary>
    /// Called when the player picks up the object
    /// </summary>
    public void OnPickup(Transform carryPoint)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.detectCollisions = false;

        transform.SetParent(carryPoint);
        transform.localPosition = carryOffset;
        transform.localRotation = Quaternion.identity;

        // Play carry audio
        if (audioSource != null && carryClip != null)
        {
            audioSource.clip = carryClip;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Called when the player drops the object
    /// </summary>
    public void OnDrop()
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.detectCollisions = true;

        // Stop carry audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnDisable()
    {
        // Ensure audio stops if object is disabled
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
