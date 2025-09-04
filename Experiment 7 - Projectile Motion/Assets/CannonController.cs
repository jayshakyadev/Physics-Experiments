using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;   // Speed of movement
    public float rotateSpeed = 100f; // Speed of rotation

    void Update()
    {
        // --- Movement ---
        float moveZ = 0f;
        float moveX = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ = 1f;   // Forward
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;  // Backward
        if (Input.GetKey(KeyCode.D)) moveX = 1f;   // Right
        if (Input.GetKey(KeyCode.A)) moveX = -1f;  // Left

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

    }
}
