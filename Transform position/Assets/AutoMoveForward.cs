using UnityEngine;

public class AutoMoveForward : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.forward; // Default direction
    public float moveSpeed = 2f; // Adjustable speed

    void Update()
    {
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
    }
}