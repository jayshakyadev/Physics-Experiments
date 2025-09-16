using UnityEngine;
//navneet prashar
//24bcg10125

public class VectorOperations : MonoBehaviour
{
    public Transform cube1;
    public Transform cube2;
    public Transform resultCube;

    private Vector3 originalPosition1;
    private Vector3 originalPosition2;

    void Start()
    {
        if (cube1 != null) originalPosition1 = cube1.position;
        if (cube2 != null) originalPosition2 = cube2.position;
    }

    void Update()
    {
        if (cube1 == null || cube2 == null || resultCube == null) return;

        Vector3 v1 = cube1.position;
        Vector3 v2 = cube2.position;

        // A - Vector Addition
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 addition = v1 + v2;
            Debug.Log("Addition: " + addition);
            resultCube.position = addition;
        }

        // S - Vector Subtraction
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 subtraction = v1 - v2;
            Debug.Log("Subtraction: " + subtraction);
            resultCube.position = subtraction;
        }

        // D - Dot Product
        if (Input.GetKeyDown(KeyCode.D))
        {
            float dot = Vector3.Dot(v1.normalized, v2.normalized);
            Debug.Log("Dot Product: " + dot);
        }

        // C - Cross Product
        if (Input.GetKeyDown(KeyCode.C))
        {
            Vector3 cross = Vector3.Cross(v1.normalized, v2.normalized);
            Debug.Log("Cross Product: " + cross);
            resultCube.position = cross * 5f; // scaling for visibility
        }

        // R - Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            cube1.position = originalPosition1;
            cube2.position = originalPosition2;
            resultCube.position = Vector3.zero;
            Debug.Log("Reset to original positions");
        }
    }
}
