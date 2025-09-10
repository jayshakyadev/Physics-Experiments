using UnityEngine;

public class CannonFire : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;               // Assign: the FirePoint (child at muzzle)
    public GameObject projectilePrefab;       // Assign: Projectile prefab (must have Rigidbody & Collider)

    [Header("Ballistics")]
    [Tooltip("Initial speed in meters/second (m/s). Try 30-60 for visible arcs.")]
    public float launchSpeed = 40f;
    [Tooltip("If true we set rb.velocity directly. If false, AddForce with VelocityChange is used.")]
    public bool setVelocityDirectly = true;

    [Header("Fire Control")]
    public float reloadTime = 0.75f;          // seconds between shots
    public float projectileLifetime = 12f;    // auto destroy after this many seconds
    public float spawnOffset = 0.12f;         // move spawn slightly forward so it doesn't intersect the barrel

    [Header("Collision Safety")]
    public bool ignoreCannonCollisions = true; // when true will ignore collisions between projectile and cannon colliders
    public Collider[] cannonCollidersToIgnore; // drag the cannon/base/nozzle colliders here in Inspector

    float _nextFireTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _nextFireTime)
            Fire();
    }

    public void Fire()
    {
        _nextFireTime = Time.time + reloadTime;

        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("CannonFire: projectilePrefab or firePoint not assigned.");
            return;
        }

        Vector3 spawnPos = firePoint.position + firePoint.forward * spawnOffset;
        Quaternion spawnRot = firePoint.rotation;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, spawnRot);

        // Ensure projectile has a collider and rigidbody
        Collider projCol = proj.GetComponent<Collider>();
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        // Prevent instant collision with the cannon (if requested)
        if (ignoreCannonCollisions && projCol != null && cannonCollidersToIgnore != null)
        {
            foreach (var c in cannonCollidersToIgnore)
                if (c != null)
                    Physics.IgnoreCollision(projCol, c, true);
        }

        if (rb != null)
        {
            // sane rb settings for projectile
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.linearDamping = 0f; // no Unity linear drag for ideal parabola
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            Vector3 initialVelocity = firePoint.forward.normalized * launchSpeed;

            if (setVelocityDirectly)
            {
                // exact initial velocity � best match to projectile motion formula
                rb.linearVelocity = initialVelocity;
            }
            else
            {
                // change velocity accounting for mass (VelocityChange ignores mass; Impulse would scale with mass)
                rb.AddForce(initialVelocity, ForceMode.VelocityChange);
            }
        }
        else
        {
            Debug.LogWarning("CannonFire: spawned projectile has no Rigidbody.");
        }

        Destroy(proj, projectileLifetime);
    }
}
