using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathControl))]
public class GridMovement : MonoBehaviour
{
    [Header("Movement")]
    public LayerMask wallColliderMask = ~0;
    [SerializeField] private bool m_pushable = true;

    [Header("Position")]
    public Vector3 gridScale = Vector3.one;
    public Vector3 gridOffset = Vector3.zero;
    public float travelTime = .1f;

    [Header("Rotation")]
    public Rotator rotator;
    public bool faceMovementDirection;
    public Vector3 rollInMovementDirection;

    protected List<GridMovement> attachedObjects = new List<GridMovement>();
    public void AddObject(GridMovement grid)
    {
        if (grid == null) return;
        attachedObjects.Add(grid);
    }
    public void RemoveObject(GridMovement grid) { attachedObjects.Remove(grid); }

    protected Rigidbody rb;
    protected Rigidbody2D rb2D;
    protected PathControl pathControl;

    public bool IsMoving { get { return pathControl.Count > 0; } }

    public bool Pushable
    {
        get
        {
            return m_pushable && pathControl.Count == 0;
        }

        set
        {
            m_pushable = value;
        }
    }

    protected void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb2D = GetComponentInParent<Rigidbody2D>();
        if (rb == null && rb2D == null)
            Warnings.ComponentMissing(this);

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

    public virtual bool Move(Vector3 v) { return Move(v, false); }
    public virtual bool Move(Vector3 v, bool fixRotation)
    {
        if (IsMoving == true)
            return false;

        Vector3 target = FindNearestGridPoint(transform.position + Vector3.Scale(gridScale, v));
        Vector3 movement = target - transform.position;

        // Check if there's something in the way
        HashSet<GridMovement> pushables = GetAffectedObjectsAlong(movement, false);
        HashSet<GridMovement> allAffected = GetAffectedObjectsAlong(movement, true);

        if (pushables.IsProperSubsetOf(allAffected))
            return false;

        foreach (GridMovement g in pushables)
            g.Push(movement, fixRotation);
        return true;
    }

    public void Push(Vector3 movement, bool fixRotation)
    {
        if (IsMoving == true)
            return;

        Vector3 target = transform.position + movement;
        if (rollInMovementDirection != Vector3.zero && fixRotation == false)
        {
            float angle = Vector3.Dot(movement, rollInMovementDirection);
            Vector3 axis = Vector3.Cross(Vector3.up, movement); // [TODO] what if we're moving Vector3.up?

            Quaternion rotation = Quaternion.AngleAxis(angle, axis) * transform.rotation;
            pathControl.AddWaypoint(new PathPoint(target, rotation, travelTime, PathMode.Time));
        }
        else
        {
            pathControl.AddWaypoint(new PathPoint(target, travelTime, PathMode.Time));

            if (faceMovementDirection && fixRotation == false)
                transform.rotation = rotator.GetRotationTowards(movement);
        }
    }

    public bool IsPushableTowards(Vector3 movement) { return IsPushableTowards(movement, true); }
    public bool IsPushableTowards(Vector3 movement, bool ignoreThisPushable)
    {
        foreach (Transform t in SweepTestAll(movement))
        {
            GridMovement g = t.GetComponent<GridMovement>();
            if (g == null || g.IsPushableTowards(movement, false) == false)
                return false;
        }
        return ignoreThisPushable || Pushable;
    }

    public HashSet<GridMovement> GetAffectedObjectsAlong(Vector3 movement, bool includeUnpushables)
    {
        Queue<GridMovement> sweepQueue = new Queue<GridMovement>();
        HashSet<GridMovement> affected = new HashSet<GridMovement>();

        sweepQueue.Enqueue(this);
        affected.Add(this);
        foreach (GridMovement grid in attachedObjects)
        {
            sweepQueue.Enqueue(grid);
            affected.Add(grid);
        }

        while (sweepQueue.Count > 0)
        {
            GridMovement current = sweepQueue.Dequeue();
            List<Transform> sweptObjects = current.SweepTestAll(movement);
            foreach (Transform t in sweptObjects)
            {
                GridMovement g = t.GetComponent<GridMovement>();
                if (g == null || g.Pushable == false)
                {
                    if (includeUnpushables == true)
                        affected.Add(g);
                    continue;
                }

                if (affected.Contains(g) == true)
                    continue;
                sweepQueue.Enqueue(g);
                affected.Add(g);
            }
        }
        return affected;
    }

    protected List<Transform> SweepTestAll(Vector3 v)
    {
        List<Transform> results = new List<Transform>();
        if (rb != null)
        {
            RaycastHit[] hits = rb.SweepTestAll(v.normalized, v.magnitude);
            foreach (RaycastHit hit in hits)
                if (wallColliderMask.Contains(hit.transform.gameObject.layer))
                    results.Add(hit.transform);
        }
        else if (rb2D != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int count = rb2D.Cast(v.normalized, hits, v.magnitude);
            for (int i = 0; i < count; i++)
                if (wallColliderMask.Contains(hits[i].transform.gameObject.layer))
                    results.Add(hits[i].transform);
        }
        return results;
    }

    public void SnapToGrid()
    {
        Move(Vector3.zero);
    }

    public Vector3 FindNearestGridPointRelative(Vector3 offset)
    {
        return FindNearestGridPoint(transform.position + offset);
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
