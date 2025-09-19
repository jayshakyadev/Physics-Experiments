using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform player;           // Player capsule to follow
    public Vector3 offset = new Vector3(0f, 2f, -4f);  // Fixed offset from the player

    void LateUpdate()
    {
        // Position the camera at the player's position plus the fixed offset
        transform.position = player.position + offset;

        // Optional: Look at the player so camera always faces them
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
