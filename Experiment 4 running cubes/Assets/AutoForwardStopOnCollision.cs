using UnityEngine;

public class AutoForwardStopOnCollision : MonoBehaviour
{
    public float moveSpeed = 5f;           // Forward movement speed
    private bool canMove = true;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
