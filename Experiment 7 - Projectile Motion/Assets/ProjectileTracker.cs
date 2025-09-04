using UnityEngine;

public class ProjectileTracker : MonoBehaviour
{
    private Vector3 startPos;
    private float pathTravelled = 0f;
    private Vector3 lastPos;
    private bool hasReported = false;

    void Start()
    {
        startPos = transform.position;
        lastPos = startPos;
    }

    void Update()
    {
        // Accumulate the actual path length (arc distance)
        pathTravelled += Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasReported) return; // avoid multiple triggers
        hasReported = true;

        // Horizontal range (ignoring height difference, like physics class)
        Vector3 flatStart = new Vector3(startPos.x, 0f, startPos.z);
        Vector3 flatHit = new Vector3(transform.position.x, 0f, transform.position.z);
        float horizontalRange = Vector3.Distance(flatStart, flatHit);

        Debug.Log($"💥 Projectile hit {collision.gameObject.name}");
        Debug.Log($"➡ Horizontal Range: {horizontalRange:F2} meters");
        Debug.Log($"🔄 Total Path Travelled: {pathTravelled:F2} meters");

        // Optionally destroy after short delay
        Destroy(gameObject, 0.2f);
    }

    void OnDestroy()
    {
        // Safety check: if destroyed by lifetime and not collision
        if (!hasReported)
        {
            Vector3 flatStart = new Vector3(startPos.x, 0f, startPos.z);
            Vector3 flatEnd = new Vector3(transform.position.x, 0f, transform.position.z);
            float horizontalRange = Vector3.Distance(flatStart, flatEnd);

            Debug.Log("⚠ Projectile destroyed (lifetime expired)");
            Debug.Log($"➡ Horizontal Range: {horizontalRange:F2} meters");
            Debug.Log($"🔄 Total Path Travelled: {pathTravelled:F2} meters");
        }
    }
}

