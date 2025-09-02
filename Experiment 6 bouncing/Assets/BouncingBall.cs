using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class BouncingBall : MonoBehaviour
{
    public PhysicsMaterial bounceMaterial;
    public Vector3 initialForce = new Vector3(0, 5, 0); // Initial force to start bounce
    public float mass = 1f; // Mass of the Rigidbody, can be set in Inspector

    private Rigidbody rb;
    private SphereCollider sphereCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();

        if (bounceMaterial != null)
        {
            sphereCollider.material = bounceMaterial;
        }

        rb.mass = mass;

        
        rb.AddForce(initialForce, ForceMode.Impulse);

        rb.useGravity = true;
    }
}
