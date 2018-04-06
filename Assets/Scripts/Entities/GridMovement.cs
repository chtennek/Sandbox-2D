using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathControl))]
public class GridMovement : MonoBehaviour
{
    [Header("Movement")]
    public LayerMask wallColliderMask = ~0;
    [SerializeField]
    private bool m_pushable = true;

    [Header("Position")]
    public Vector3 gridScale = Vector3.one;
    public Vector3 gridOffset = Vector3.zero;
    public float travelTime = .1f;

    [Header("Rotation")]
    public Rotator rotator;
    public bool faceMovementDirection;
    public Vector3 rollInMovementDirection;

    private Rigidbody rb;
    private Rigidbody2D rb2D;
    private PathControl pathControl;

	public bool Pushable
    {
        get
        {
            return m_pushable;
        }

        set
        {
            m_pushable = value;
        }
    }

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb2D = GetComponentInParent<Rigidbody2D>();
        if (rb == null && rb2D == null)
            Debug.Log("No Rigidbody attached to GridMovement. Pushable behaviour won't work properly!");

        pathControl = GetComponent<PathControl>();
    }

    public bool RotateTowards(Vector3 direction, Vector3 relativePivot)
    {
        if (pathControl.Count > 0 || direction == Vector3.zero)
            return false;

        Vector3 pivot = transform.position + relativePivot;
        Quaternion rotation = rotator.GetRotationTowards(direction);
        Vector3 rotatedPosition = Mathv.RotateAround(transform.position, pivot, rotation);

        if (rotatedPosition == transform.position && Quaternion.Angle(rotation, transform.rotation) < Mathf.Epsilon)
            return false;

        pathControl.AddWaypoint(new PathPoint(rotatedPosition, rotation, travelTime, PathMode.Time));
        return true;
    }

    public bool RotateAround(Vector3 pivot, Quaternion rotation)
    {
        // Check if we're moving
        if (pathControl.Count > 0 || Quaternion.Angle(rotation, Quaternion.identity) < Mathf.Epsilon)
            return false;

        Vector3 rotatedPosition = Mathv.RotateAround(transform.position, pivot, rotation);
        pathControl.AddWaypoint(new PathPoint(rotatedPosition, rotation, travelTime, PathMode.Time));
        return true;
    }

    public bool Move(Vector3 v) { return Move(v, false); }
    public bool Move(Vector3 v, bool fixRotation)
    {
        // Check if we're moving
        if (pathControl.Count > 0) // || v == Vector3.zero)
            return false;

        Vector3 target = FindNearestGridPoint(transform.position + Vector3.Scale(gridScale, v));
        Vector3 movement = target - transform.position;

        // Check if there's something in the way
        if (IsPushableTowards(movement) == false)
            return false;

        Push(movement, fixRotation);
        return true;
    }

    // [TODO] don't assume we've already checked IsPushable?
    // [TODO] add running checklist to avoid infinite loops in weird cases
    private void Push(Vector3 movement, bool fixRotation)
    {
        Vector3 target = transform.position + movement;
        if (rollInMovementDirection != Vector3.zero && fixRotation == false)
        {
            float angle = Vector3.Dot(movement, rollInMovementDirection);
            Vector3 axis = Vector3.Cross(Vector3.up, movement);

            // [TODO] what if we're moving Vector3.up?
            Quaternion rotation = Quaternion.AngleAxis(angle, axis) * transform.rotation;
            pathControl.AddWaypoint(new PathPoint(target, rotation, travelTime, PathMode.Time));
        }
        else
        {
            pathControl.AddWaypoint(new PathPoint(target, travelTime, PathMode.Time));

            if (faceMovementDirection && fixRotation == false)
                transform.rotation = rotator.GetRotationTowards(movement);
        }

        foreach (Transform t in SweepTestAll(movement))
        {
            GridMovement g = t.GetComponent<GridMovement>();
            if (g != null && g.Pushable == true)
                g.Push(movement, fixRotation);
        }
    }

    public bool IsPushableTowards(Vector3 movement)
    {
        foreach (Transform t in SweepTestAll(movement))
        {
            GridMovement g = t.GetComponent<GridMovement>();
            if (g == null || g.IsPushableTowards(movement) == false)
                return false;
        }
        return Pushable;
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

    public Vector3 FindNearestGridPoint(Vector3 position)
    {
        Vector3 inverseGridSize = new Vector3(1 / gridScale.x, 1 / gridScale.y, 1 / gridScale.z);
        Vector3 normalizedPosition = Vector3.Scale(inverseGridSize, position - gridOffset);
        normalizedPosition.x = Mathf.Round(normalizedPosition.x);
        normalizedPosition.y = Mathf.Round(normalizedPosition.y);
        normalizedPosition.z = Mathf.Round(normalizedPosition.z);
        return Vector3.Scale(gridScale, normalizedPosition) + gridOffset;
    }
}
