using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravity = -9.81f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Disable built-in gravity since we are applying it manually
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        Vector3 gravityForce = new Vector3(0, gravity * rb.mass, 0);
        rb.AddForce(gravityForce);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
    }
}
