using UnityEngine;

public class AutoForwardNoGravity : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private bool stopMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;  // Disable gravity
        rb.linearVelocity = transform.forward * speed;
    }

    void FixedUpdate()
    {
        if (!stopMoving)
        {
            rb.linearVelocity = transform.forward * speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            return;

        stopMoving = true;
        rb.linearVelocity = Vector3.zero;
        Debug.Log("Ball stopped: collided with " + collision.gameObject.name + "; Speed = 0");
    }
}
