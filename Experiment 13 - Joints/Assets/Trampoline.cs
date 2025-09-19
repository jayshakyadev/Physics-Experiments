using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] float trampolineForce = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        // FILTERED USING LAYER COLLISION MATRIX
        if (!collision.rigidbody)
            return;
        if (collision.contacts[0].normal.y >= 0.9f) // CHECK IF COLLISION FROM ABOVE
            return;

        collision.rigidbody.AddForce(Vector3.up * trampolineForce, ForceMode.Impulse);
    }
}
