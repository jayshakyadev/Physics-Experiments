using UnityEngine;

public class StopOnCollision : MonoBehaviour
{
    public float speed = 5f;   // starting speed
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;  // move forward in facing direction
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignore collision with Plane
        if (collision.gameObject.name == "Plane" || collision.gameObject.CompareTag("Ground"))
            return;

        // Stop this object
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Stop the other object if it has a Rigidbody
        Rigidbody otherRb = collision.rigidbody;
        if (otherRb != null)
        {
            otherRb.linearVelocity = Vector3.zero;
            otherRb.angularVelocity = Vector3.zero;
        }

    }
}
