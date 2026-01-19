using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 positionOffset = new Vector3(0f, 2f, -5f);
    [SerializeField] private float followSpeed = 10f;

    private void LateUpdate()
    {
        if (player == null)
            return;

        Vector3 desiredPosition = player.position + positionOffset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}
