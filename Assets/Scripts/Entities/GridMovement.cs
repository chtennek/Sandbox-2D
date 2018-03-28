using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathControl))]
public class GridMovement : MonoBehaviour
{
    [Header("Movement")]
    public LayerMask wallColliderMask;
    public bool pushable = true;

    [Header("Position")]
    public Vector3 gridScale = Vector3.one;
    public Vector3 gridOffset = Vector3.zero;
    public float travelTime = .1f;

    [Header("Rotation")]
    public Rotator rotator;
    public bool faceMovementDirection;
    public bool rollInMovementDirection;

    private Rigidbody rb;
    private Rigidbody2D rb2D;
    private PathControl pathControl;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb2D = GetComponentInParent<Rigidbody2D>();
        if (rb == null && rb2D == null)
            Debug.Log("No Rigidbody attached to GridMovement. Pushable behaviour won't work properly!");

        pathControl = GetComponent<PathControl>();
    }

    public void RotateTowards(Vector3 v)
    {

    }

    public void Move(Vector3 v)
    {
        // Check if we're moving
        if (pathControl.Count > 0 || v == Vector3.zero)
            return;

        Vector3 target = FindNearestGridPoint(transform.position + Vector3.Scale(gridScale, v));
        Vector3 movement = target - transform.position;

        // Check if there's something in the way
        if (IsPushableTowards(movement) == false)
            return;

        Push(movement);
    }

    // [TODO] don't assume we've already checked IsPushable?
    // [TODO] add running checklist to avoid infinite loops in weird cases
    private void Push(Vector3 movement)
    {
        Vector3 target = transform.position + movement;
        if (rollInMovementDirection)
        {
            Quaternion rotation = Quaternion.AngleAxis(90, Vector3.Cross(Vector3.up, movement)) * transform.rotation;
            pathControl.AddWaypoint(new PathPoint(target, rotation, 1 / travelTime));
        }
        else
        {
            pathControl.AddWaypoint(new PathPoint(target, 1 / travelTime));

            if (faceMovementDirection)
                transform.rotation = rotator.GetRotationTowards(movement);
        }

        foreach (Transform t in SweepTestAll(movement))
        {
            GridMovement g = t.GetComponent<GridMovement>();
            if (g != null && g.pushable == true)
                g.Push(movement);
        }
    }

    private bool IsPushableTowards(Vector3 movement)
    {
        foreach (Transform t in SweepTestAll(movement))
        {
            GridMovement g = t.GetComponent<GridMovement>();
            if (g == null || g.IsPushableTowards(movement) == false)
                return false;
        }
        return pushable;
    }

    private List<Transform> SweepTestAll(Vector3 v)
    {
        List<Transform> results = new List<Transform>();
        if (rb != null)
        {
            RaycastHit[] hits = rb.SweepTestAll(v.normalized, v.magnitude);
            foreach (RaycastHit hit in hits)
            {
                results.Add(hit.transform);
            }
        }
        else if (rb2D != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int count = rb2D.Cast(v.normalized, hits, v.magnitude);
            for (int i = 0; i < count; i++)
            {
                results.Add(hits[i].transform);
            }
        }
        return results;
    }

    private Vector3 FindNearestGridPoint(Vector3 position)
    {
        Vector3 inverseGridSize = new Vector3(1 / gridScale.x, 1 / gridScale.y, 1 / gridScale.z);
        Vector3 normalizedPosition = Vector3.Scale(inverseGridSize, position - gridOffset);
        normalizedPosition.x = Mathf.Round(normalizedPosition.x);
        normalizedPosition.y = Mathf.Round(normalizedPosition.y);
        normalizedPosition.z = Mathf.Round(normalizedPosition.z);
        return Vector3.Scale(gridScale, normalizedPosition) + gridOffset;
    }
}
