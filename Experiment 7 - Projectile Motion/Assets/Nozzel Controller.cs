using UnityEngine;

public class NozzleController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;
    public float minPitch = 0f;
    public float maxPitch = 60f;

    [Range(0f, 360f)] public float yawAngle = 0f;  // left/right rotation in inspector

    private float currentPitch = 0f;   // up/down tilt

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Left mouse button = pitch (up & down)
        if (Input.GetMouseButton(0) && scroll != 0)
        {
            currentPitch += scroll * rotationSpeed;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }

        // Right mouse button = yaw (left & right, full 360°)
        if (Input.GetMouseButton(1) && scroll != 0)
        {
            yawAngle += scroll * rotationSpeed;
            if (yawAngle > 360f) yawAngle -= 360f;
            if (yawAngle < 0f) yawAngle += 360f;
        }

        // Apply both rotations (pitch on X, yaw on Y)
        transform.localRotation = Quaternion.Euler(-currentPitch, yawAngle, 0f);
    }
}
