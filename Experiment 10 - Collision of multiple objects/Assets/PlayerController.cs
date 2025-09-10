using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down

        // Movement vector
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed;

        // Apply movement using physics
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }
}