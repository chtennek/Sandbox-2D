using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public enum GridDirection
{
    None,
    Up,
    Left,
    Down,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
}

public class GridEntity : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float duration = .5f;
    [SerializeField] private bool durationPerDistance = true;

    [Header("References")]
    public GridSettings grid;
    [SerializeField] private Transform visual;
    [SerializeField] private Rigidbody rb; // [TODO] Use rigidbody instead of transform to sweep for push/collision logic

    [Tooltip("Entity can't occupy a point if occupied by a collider matching blacklistMask.")]
    [SerializeField] private CompoundMask blacklistMask;

    [Tooltip("Entity can't occupy a point unless already occupied by a collider matching whitelistMask.")]
    [SerializeField] private CompoundMask whitelistMask;
    [SerializeField] private bool useWhitelist;

    private Vector3Int m_gridPosition;
    public Vector3Int GridPosition
    {
        get { return m_gridPosition; }
        set { WarpTo(value); }
    }

    private Vector3 LogicalPosition // GridPosition transformed to world space
    {
        get
        {
            if (rb != null) return rb.transform.position;
            return transform.position;
        }
        set
        {
            if (rb != null) rb.transform.position = value;
            else transform.position = value;
        }
    }

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        // Start by locking ourselves to the nearest grid position in world space
        m_gridPosition = grid.ToGridSpace(LogicalPosition);
        Debug.Log(transform);
        Debug.Log(m_gridPosition);
    }

    public void Warp(float x, float y, float z) { Warp(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void Warp(int x, int y, int z) { Warp(new Vector3Int(x, y, z)); }
    public void Warp(Vector3Int offset)
    {
        if (offset != Vector3Int.zero)
            WarpTo(GridPosition + offset);
    }

    public void WarpTo(float x, float y, float z) { WarpTo(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void WarpTo(int x, int y, int z) { WarpTo(new Vector3Int(x, y, z)); }
    public void WarpTo(Vector3Int point)
    {
        if (CanOccupy(point) == false)
            return;

        if (visual != null)
            visual.transform.position = LogicalPosition;

        m_gridPosition = point;
        LogicalPosition = grid.ToWorldSpace(point);
    }

    public void Move(float x, float y, float z) { Move(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void Move(int x, int y, int z) { Move(new Vector3Int(x, y, z)); }
    public void Move(Vector3Int offset)
    {
        if (offset != Vector3Int.zero)
            MoveTo(GridPosition + offset);
    }

    public void MoveTo(float x, float y, float z) { MoveTo(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void MoveTo(int x, int y, int z) { MoveTo(new Vector3Int(x, y, z)); }
    public void MoveTo(Vector3Int point, bool orthogonalPath = true)
    {
        if (orthogonalPath == false)
        {
            MoveThrough(new Vector3Int[] { point });
            return;
        }

        Vector3Int waypointX = new Vector3Int(point.x, GridPosition.y, GridPosition.z);
        Vector3Int waypointY = new Vector3Int(point.x, point.y, GridPosition.z);
        Vector3Int waypointZ = new Vector3Int(point.x, point.y, point.z);
        MoveThrough(new Vector3Int[] { waypointX, waypointY, waypointZ, point });
    }

    public void MoveThrough(Vector3Int[] points)
    {
        if (points.Length == 0 || CanOccupy(points[points.Length - 1]) == false)
            return;

        // Build tween path for visual to move through
        if (visual != null)
        {
            Vector3[] path = new Vector3[points.Length];
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = grid.ToWorldSpace(points[i]);
                Debug.Log(path[i]);
            }

            float calculatedDuration = duration;
            if (durationPerDistance)
                calculatedDuration *= CalculatePathDistance(path);

            visual.DOPath(path, calculatedDuration);
        }

        Vector3Int point = points[points.Length - 1];
        m_gridPosition = point;
        LogicalPosition = grid.ToWorldSpace(point);
    }

    private float CalculatePathDistance(Vector3[] path)
    {
        float distance = 0;
        Vector3 origin = LogicalPosition;
        foreach (Vector3 waypoint in path)
        {
            distance += Vector3.Distance(waypoint, origin);
            origin = waypoint;
        }
        return distance;
    }

    private bool CanOccupy(Vector3Int point)
    {
        List<Transform> blacklist = blacklistMask.GetCollidersWithin(0, grid.ToWorldSpace(point));

        if (blacklist.Count > 0)
            return false;

        if (useWhitelist == false)
            return true;

        List<Transform> whitelist = whitelistMask.GetCollidersWithin(0, grid.ToWorldSpace(point));
        return whitelist.Count > 0;
    }
}
