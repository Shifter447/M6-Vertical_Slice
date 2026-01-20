using UnityEngine;

public class GooseHeadLook : MonoBehaviour
{
    [Header("References")]
    public Transform head;          // Assign head bone
    public GooseDrag drag;

    [Header("Look Settings")]
    public float turnSpeed = 6f;
    public float maxYaw = 60f;
    public float maxPitch = 25f;

    private Quaternion initialLocalRotation;

    void Awake()
    {
        if (head != null)
            initialLocalRotation = head.localRotation;
    }

    void LateUpdate()
    {
        if (head == null || drag == null)
            return;

        Quaternion targetRotation = initialLocalRotation;

        if (drag.IsDragging())
        {
            Rigidbody dragged = drag.GetGrabbedRigidbody();
            if (dragged != null)
            {
                Vector3 worldDir = dragged.position - head.position;
                Vector3 localDir = head.parent.InverseTransformDirection(worldDir.normalized);

                float yaw = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;
                float pitch = -Mathf.Asin(localDir.y) * Mathf.Rad2Deg;

                yaw = Mathf.Clamp(yaw, -maxYaw, maxYaw);
                pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

                targetRotation =
                    Quaternion.Euler(pitch, yaw, 0f) * initialLocalRotation;
            }
        }

        head.localRotation = Quaternion.Slerp(
            head.localRotation,
            targetRotation,
            Time.deltaTime * turnSpeed
        );
    }
}
