using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed

    private Rigidbody rb;
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent physics rotation
    }

    void Update()
    {
        // Read WASD input directly (simple method)
        input = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;
        input = input.normalized;
    }

    void FixedUpdate()
    {
        // World-space movement
        Vector3 moveDir = new Vector3(input.x, 0f, input.y);
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = rb.linearVelocity.y; // Preserve vertical motion

        rb.linearVelocity = velocity;
    }
}

