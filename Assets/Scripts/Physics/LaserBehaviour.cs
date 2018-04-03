using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LaserBehaviour : MonoBehaviour
{
    public float length = 10f; // Don't set to infinity
    public float speed = Mathf.Infinity;

    [Header("Reflection")]
    public LayerMask mask = ~0;
    public int maxReflects = 0;

    private float currentLength;
    private Vector3[] positions;
    private LineRenderer[] lines;

	private void Start()
    {
        lines = GetComponentsInChildren<LineRenderer>();
        positions = new Vector3[4 + maxReflects];
    }

    private void Update()
    {
        if (currentLength < length)
            currentLength = Mathf.Min(length, currentLength + speed * Time.deltaTime);

        if (Application.isPlaying == true || transform.hasChanged)
            CalculatePositions();
    }

    public void CalculatePositions()
    {
        float workingLength = currentLength;
        Vector3 currentPosition = transform.position;
        Vector3 currentDirection = transform.rotation * Vector3.right;
        int i = 0;

        // Reflect laser against walls
        if (maxReflects > 0)
        {
            if (positions.Length != 4 + maxReflects)
                positions = new Vector3[4 + maxReflects];

            RaycastHit hit;
            for (i = 0; i < maxReflects; i++)
            {
                if (Physics.Raycast(currentPosition, currentDirection, out hit, workingLength, mask) == false)
                    break;

                currentPosition = hit.point;
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                workingLength -= hit.distance;

                positions[i + 2] = transform.InverseTransformPoint(currentPosition);
            }
        }

        // Populate the rest of positions[]
        currentPosition += currentDirection * workingLength;
        Debug.Log(currentPosition);
        for (int j = i + 2; j < positions.Length; j++)
        {
            positions[j] = transform.InverseTransformPoint(currentPosition);
        }

        SetPositions(positions);
    }

    public void SetPosition(int index, Vector3 position)
    {
        foreach (LineRenderer line in lines)
        {
            line.SetPosition(index, position);
        }
    }

    public void SetPositions(Vector3[] positions)
    {
        foreach (LineRenderer line in lines)
        {
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }
}
