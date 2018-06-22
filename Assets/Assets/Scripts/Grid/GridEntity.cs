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

public class GridEntity : MonoBehaviour, IPrioritizable
{
    [Header("Execution Order")]
    [SerializeField] protected bool register = false;
    public int order = 0;
    [SerializeField] protected int priority = 0;
    public int Priority { get { return priority; } set { priority = value; } }

    [Header("Properties")]
    [SerializeField] protected float duration = .5f;
    [SerializeField] protected bool durationPerDistance = true;

    [Header("References")]
    public GridSettings grid;
    [SerializeField] protected Transform visual;
    [SerializeField] protected Rigidbody logical; // [TODO] Use rigidbody instead of transform to sweep for push/collision logic

    [Tooltip("Entity can't occupy a point if occupied by a collider matching blacklistMask.")]
    [SerializeField] protected CompoundMask blacklistMask;

    [Tooltip("Entity can't occupy a point unless already occupied by a collider matching whitelistMask.")]
    [SerializeField] protected CompoundMask whitelistMask;
    [SerializeField] protected bool useWhitelist;

    protected Vector3Int m_gridPosition;
    public Vector3Int GridPosition
    {
        get { return m_gridPosition; }
        set { WarpTo(value); }
    }

    protected Vector3 LogicalPosition // GridPosition transformed to world space
    {
        get
        {
            if (logical != null) return logical.transform.position;
            return transform.position;
        }
        set
        {
            if (logical != null) logical.transform.position = value;
            else transform.position = value;
        }
    }

    protected void Reset()
    {
        logical = GetComponent<Rigidbody>();
    }

    protected virtual void Awake()
    {
        // Start by locking ourselves to the nearest grid position in world space
        WarpTo(grid.ToGridSpace(LogicalPosition), force: true);
    }

    protected virtual void Start()
    {
        if (register && GridManager.main != null)
            GridManager.main.Register(this);
    }

    protected virtual void OnDisable()
    {
        if (register && GridManager.main != null)
            GridManager.main.Deregister(this);
    }

    public void Warp(float x, float y, float z) { Warp(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void Warp(int x, int y, int z) { Warp(new Vector3Int(x, y, z)); }
    public virtual void Warp(Vector3Int offset, bool force = false, bool moveVisual = true)
    {
        if (offset != Vector3Int.zero)
            WarpTo(GridPosition + offset, force, moveVisual);
    }

    // Ignore collision/CanOccupy restrictions
    public void WarpTo(float x, float y, float z) { WarpTo(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void WarpTo(int x, int y, int z) { WarpTo(new Vector3Int(x, y, z)); }
    public void WarpTo(Vector3Int point, bool force = false, bool moveVisual = true)
    {
        if (force == false && CanOccupy(point) == false)
            return;

        m_gridPosition = point;
        LogicalPosition = grid.ToWorldSpace(point);

        if (moveVisual && visual != null)
            visual.transform.position = LogicalPosition;
    }

    public void SubmitMove(float x, float y, float z) { SubmitMove(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void SubmitMove(int x, int y, int z) { SubmitMove(new Vector3Int(x, y, z)); }
    public void SubmitMove(Vector3Int move)
    {
        GridManager.main.SubmitMove(this, move);
    }

    public void Move(float x, float y, float z) { Move(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void Move(int x, int y, int z) { Move(new Vector3Int(x, y, z)); }
    public void Move(Vector3Int offset, bool force = false, bool orthogonalPath = true)
    {
        if (offset != Vector3Int.zero)
            MoveTo(GridPosition + offset, force, orthogonalPath);
    }

    public void MoveTo(float x, float y, float z) { MoveTo(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void MoveTo(int x, int y, int z) { MoveTo(new Vector3Int(x, y, z)); }
    public void MoveTo(Vector3Int point, bool force = false, bool orthogonalPath = true)
    {
        if (orthogonalPath == false)
        {
            MoveThrough(new Vector3Int[] { point });
            return;
        }

        Vector3Int waypointX = new Vector3Int(point.x, GridPosition.y, GridPosition.z);
        Vector3Int waypointY = new Vector3Int(point.x, point.y, GridPosition.z);
        Vector3Int waypointZ = new Vector3Int(point.x, point.y, point.z);
        MoveThrough(new Vector3Int[] { GridPosition, waypointX, waypointY, waypointZ, point }, force);
    }

    public void MoveThrough(Vector3Int[] points, bool force = false)
    {
        if (force == false && (points.Length == 0 || CanOccupy(points[points.Length - 1]) == false))
            return;

        Vector3Int point = points[points.Length - 1];
        m_gridPosition = point;
        LogicalPosition = grid.ToWorldSpace(point);

        // Build tween path for visual to move through
        if (visual != null)
        {
            Vector3[] path = new Vector3[points.Length];
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = grid.ToWorldSpace(points[i]);
            }

            float calculatedDuration = duration;
            if (durationPerDistance)
                calculatedDuration *= CalculatePathDistance(path);

            visual.DOPath(path, calculatedDuration);
        }
    }

    protected float CalculatePathDistance(Vector3[] path)
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

    public bool IsCurrentPositionLegal() { return CanOccupy(GridPosition); }
    public bool CanOccupy(Vector3Int point)
    {
        List<Transform> blacklist = blacklistMask.GetCollidersWithin(0, grid.ToWorldSpace(point), logical.transform);

        if (blacklist.Count > 0)
            return false;

        if (useWhitelist == false)
            return true;

        List<Transform> whitelist = whitelistMask.GetCollidersWithin(0, grid.ToWorldSpace(point), logical.transform);
        return whitelist.Count > 0;
    }
}
