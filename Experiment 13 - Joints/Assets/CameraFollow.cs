using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;              // Capsule (player) to follow
    public float distance = 5.0f;         // Camera distance from player
    public float xSpeed = 120.0f;         // Mouse X sensitivity
    public float ySpeed = 80.0f;          // Mouse Y sensitivity
    public float yMinLimit = -20f;        // Minimum Y angle
    public float yMaxLimit = 80f;         // Maximum Y angle

    float x = 0.0f;
    float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        Cursor.lockState = CursorLockMode.Locked; // Locks cursor to center screen
    }

    void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * distance);

        transform.rotation = rotation;
        transform.position = position;
    }
}
