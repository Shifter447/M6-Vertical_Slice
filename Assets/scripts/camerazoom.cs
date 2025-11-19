using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 12f;

    [Header("Focus Mode")]
    public bool enableFocusMode = true;
    public float focusZoom = 4f;
    public float focusSpeed = 6f;

    private Camera cam;
    private float targetZoom;

    private InputAction focusAction;
    private InputAction scrollAction;

    void OnEnable()
    {
        scrollAction = new InputAction(
            type: InputActionType.Value,
            binding: "<Mouse>/scroll"
        );
        scrollAction.Enable();

        focusAction = new InputAction(
            type: InputActionType.Button,
            binding: "<Keyboard>/leftShift"
        );
        focusAction.Enable();
    }

    void OnDisable()
    {
        scrollAction.Disable();
        focusAction.Disable();
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;
    }

    void Update()
    {
        HandleScrollZoom();
        HandleFocusMode();
        ApplyZoom();
    }

    void HandleScrollZoom()
    {
        Vector2 scroll = scrollAction.ReadValue<Vector2>();
        float scrollDelta = scroll.y * 0.01f;

        if (Mathf.Abs(scrollDelta) > 0.001f)
        {
            targetZoom -= scrollDelta * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    void HandleFocusMode()
    {
        if (!enableFocusMode) return;

        if (focusAction.IsPressed())
        {
            targetZoom = Mathf.Lerp(targetZoom, focusZoom, Time.deltaTime * focusSpeed);
        }
    }

    void ApplyZoom()
    {
        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                targetZoom,
                Time.deltaTime * zoomSpeed
            );
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(
                cam.fieldOfView,
                targetZoom,
                Time.deltaTime * zoomSpeed
            );
        }
    }
}
