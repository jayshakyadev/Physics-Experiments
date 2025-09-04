using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Draws a trajectory preview from firePoint using either analytic (no drag) or
/// numerical simulation (quadratic drag). Also optionally places an impact marker and reports time/distance.
/// Attach to the same GameObject as your LineRenderer (or assign one).
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPreview : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;              // muzzle tip (must be assigned)
    public LineRenderer lineRenderer;        // optional (will GetComponent if empty)
    public Transform impactMarker;           // optional small sphere to show predicted hit point

    [Header("Launch")]
    [Tooltip("Initial speed in m/s (matches the nozzle fire speed)")]
    public float launchSpeed = 40f;

    [Header("Mode")]
    [Tooltip("If true: simulate quadratic air drag numerically. If false: use analytic (ideal parabola).")]
    public bool useQuadraticDrag = false;

    [Header("Aerodynamics (for numerical mode)")]
    public float airDensity = 1.225f;           // kg/m^3
    public float dragCoefficient = 0.47f;       // sphere ~0.47
    public float projectileDiameter = 0.2f;     // meters (match prefab scale)
    public float projectileMass = 1f;           // kg (match prefab), used for drag acceleration

    [Header("Sampling / performance")]
    public int maxSteps = 200;          // points in line (cap)
    public float stepDt = 0.02f;        // time per integration step (s). Lower -> more accurate
    public LayerMask collisionMask = ~0; // layers considered for collision (default everything)

    [Header("Display")]
    public bool showImpactMarker = true;
    public Text debugText;              // optional UI text to show distance/time

    // internal
    float crossSectionArea;

    void Awake()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        // compute cross-sectional area of a sphere
        float r = Mathf.Max(0.00001f, projectileDiameter * 0.5f);
        crossSectionArea = Mathf.PI * r * r;
        // safety
        if (firePoint == null) Debug.LogWarning("TrajectoryPreview: firePoint is not assigned.");
    }

    void Update()
    {
        if (firePoint == null || lineRenderer == null) return;
        if (useQuadraticDrag)
            SimulateWithDrag();
        else
            DrawAnalytic();
    }

    // --- Analytic (no drag) sampling using kinematic equations
    void DrawAnalytic()
    {
        Vector3 p0 = firePoint.position;
        Vector3 v0 = firePoint.forward.normalized * launchSpeed;

        // approximate total flight time: solve for y(t) = ground level using raycast-free simple assumption y=0 plane at same height as p0.y
        // more robust: sample until we cross ground or hit collider
        lineRenderer.positionCount = maxSteps;
        Vector3 p = p0;
        Vector3 v = v0;
        float t = 0f;

        int idx = 0;
        for (int i = 0; i < maxSteps; i++)
        {
            // place point
            lineRenderer.SetPosition(idx++, p);

            // step forward analytically: use small dt
            float dt = stepDt;
            Vector3 pNext = p + v * dt + 0.5f * Physics.gravity * dt * dt;
            Vector3 vNext = v + Physics.gravity * dt;

            // check collision between p and pNext
            Vector3 dir = pNext - p;
            float dist = dir.magnitude;
            if (dist > 1e-5f)
            {
                if (Physics.Raycast(p, dir.normalized, out RaycastHit hit, dist, collisionMask, QueryTriggerInteraction.Ignore))
                {
                    lineRenderer.positionCount = idx + 1;
                    lineRenderer.SetPosition(idx, hit.point);
                    PlaceImpactMarker(hit.point, t + dt);
                    ReportDebug(hit.point, t + dt);
                    return;
                }
            }

            p = pNext;
            v = vNext;
            t += dt;
            // stop if below some reasonable plane (ground plane near y = p0.y - 0.1)
            if (p.y < p0.y - 0.5f && i > 2)
            {
                lineRenderer.positionCount = idx;
                PlaceImpactMarker(p, t);
                ReportDebug(p, t);
                return;
            }
        }
        // if we run out of steps
        lineRenderer.positionCount = idx;
        PlaceImpactMarker(lineRenderer.GetPosition(idx - 1), t);
        ReportDebug(lineRenderer.GetPosition(idx - 1), t);
    }

    // --- Numerical integration with quadratic drag: semi-implicit Euler
    void SimulateWithDrag()
    {
        Vector3 p = firePoint.position;
        Vector3 v = firePoint.forward.normalized * launchSpeed;
        float dt = stepDt;
        float t = 0f;

        int capacity = Mathf.Clamp(maxSteps, 4, 2000);
        lineRenderer.positionCount = capacity;

        int idx = 0;
        for (int i = 0; i < capacity; i++)
        {
            lineRenderer.SetPosition(idx++, p);

            // compute acceleration from gravity + quadratic drag: Fd = -0.5 * rho * Cd * A * v^2 * (v/|v|)
            Vector3 a = Physics.gravity;
            float speed = v.magnitude;
            if (speed > 1e-6f)
            {
                float dragMag = 0.5f * airDensity * dragCoefficient * crossSectionArea * speed * speed;
                Vector3 dragAcc = -(dragMag / Mathf.Max(1e-6f, projectileMass)) * (v / speed);
                a += dragAcc;
            }

            // semi-implicit Euler step (more stable than explicit)
            v += a * dt;
            Vector3 pNext = p + v * dt;

            // collision test between p and pNext
            Vector3 dir = pNext - p;
            float dist = dir.magnitude;
            if (dist > 1e-6f)
            {
                if (Physics.Raycast(p, dir.normalized, out RaycastHit hit, dist, collisionMask, QueryTriggerInteraction.Ignore))
                {
                    lineRenderer.positionCount = idx + 1;
                    lineRenderer.SetPosition(idx, hit.point);
                    PlaceImpactMarker(hit.point, t + dt);
                    ReportDebug(hit.point, t + dt);
                    return;
                }
            }

            p = pNext;
            t += dt;

            // early exit if we've fallen far below the starting height
            if (p.y < firePoint.position.y - 2f)
            {
                lineRenderer.positionCount = idx;
                PlaceImpactMarker(p, t);
                ReportDebug(p, t);
                return;
            }
        }

        // reached max steps — set final predicted point
        lineRenderer.positionCount = idx;
        PlaceImpactMarker(lineRenderer.GetPosition(idx - 1), t);
        ReportDebug(lineRenderer.GetPosition(idx - 1), t);
    }

    void PlaceImpactMarker(Vector3 position, float time)
    {
        if (impactMarker != null && showImpactMarker)
        {
            impactMarker.position = position;
            impactMarker.gameObject.SetActive(true);
        }
        else if (impactMarker != null)
        {
            impactMarker.gameObject.SetActive(false);
        }
    }

    void ReportDebug(Vector3 impactPoint, float time)
    {
        if (debugText != null)
        {
            float horizontalDist = Vector3.Distance(new Vector3(firePoint.position.x, 0, firePoint.position.z),
                                                    new Vector3(impactPoint.x, 0, impactPoint.z));
            debugText.text = $"Impact at t={time:F2}s, range={horizontalDist:F2}m";
        }
    }

    // small helper to visualize when inspector values change in editor
#if UNITY_EDITOR
    void OnValidate()
    {
        float r = Mathf.Max(0.00001f, projectileDiameter * 0.5f);
        crossSectionArea = Mathf.PI * r * r;
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
    }
#endif
}
