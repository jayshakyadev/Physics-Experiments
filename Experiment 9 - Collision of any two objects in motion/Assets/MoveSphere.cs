using UnityEngine;

public class MoveSphere : MonoBehaviour
{
    public float speed = 10f;   // Set in Inspector
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;  // Push sphere forward
    }
}