﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class LaserBehaviour : MonoBehaviour
{
    public float length = 10f; // Don't set to infinity
    public float speed = Mathf.Infinity;

    [Header("Collision")]
    public LayerMask refractMask;
    public LayerMask absorbMask;
    public LayerMask reflectMask = ~0;
    public int maxReflects = 0;

    public UnityEvent onRefract;
    public UnityEvent onAbsorb;
    public UnityEvent onReflect;

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
                // Find next reflection/absorption point
                if (Physics.Raycast(currentPosition, currentDirection, out hit, workingLength, absorbMask | reflectMask) == false)
                {
                    currentPosition += currentDirection * workingLength;
                    break;
                }

                // Send collision events
                foreach (RaycastHit h in Physics.RaycastAll(currentPosition, currentDirection, hit.distance, refractMask))
                {
                    CollideWith(h.transform);
                }
                CollideWith(hit.transform);

                // Update values for next raycast
                currentPosition = hit.point;
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                workingLength -= hit.distance;

                if (absorbMask.Contains(hit.transform.gameObject.layer))
                    break;

                // Update LineRenderer points
                positions[i + 2] = transform.InverseTransformPoint(currentPosition);
            }
        }

        // Populate remaining LineRenderer points
        for (int j = i + 2; j < positions.Length; j++)
        {
            positions[j] = transform.InverseTransformPoint(currentPosition);
        }

        SetPositions(positions);
    }

    private void CollideWith(Transform t) {
        CollideTrigger trigger = t.GetComponent<CollideTrigger>();
        if (trigger != null)
            trigger.onActive.Invoke();
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
