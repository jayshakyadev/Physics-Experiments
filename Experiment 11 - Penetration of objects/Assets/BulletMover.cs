using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody rb;
    private bool hasCollided = false;

    [Range(0f, 1f)]
    public float penetrationDepth = 0.5f; // how far inside the bob (as fraction of bullet size)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;  // ? Prevent multiple triggers
        hasCollided = true;

        if (collision.gameObject.CompareTag("Bob"))
        {
            Rigidbody bobRb = collision.gameObject.GetComponentInParent<Rigidbody>();

            if (bobRb != null)
            {
                // ? Apply impulse once
                Vector3 impulse = rb.mass * rb.linearVelocity;
                bobRb.AddForceAtPosition(impulse, collision.contacts[0].point, ForceMode.Impulse);

                // ? Calculate penetration
                ContactPoint contact = collision.contacts[0];
                Vector3 penetrationOffset = -contact.normal * (transform.localScale.z * penetrationDepth);

                // ? Move bullet slightly inside the bob
                transform.position = contact.point + penetrationOffset;

                // ? Stick bullet to bob
                rb.isKinematic = true;
                rb.detectCollisions = false;
                transform.SetParent(collision.transform);

                Debug.Log("[Ballistic Pendulum] Bullet embedded inside bob.");
            }
        }
    }
}
