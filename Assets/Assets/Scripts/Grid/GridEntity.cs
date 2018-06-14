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
    [SerializeField] private GridSettings grid;
    [SerializeField] private Transform visual;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CompoundMask mask;

    private Vector3Int _gridPosition;
    public Vector3Int GridPosition
    {
        get { return _gridPosition; }
        set
        {
            _gridPosition = value;
            WarpTo(value);
        }
    }

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        GridPosition = grid.ToGridSpace(rb.transform.position);
    }

    public void Warp(float x, float y, float z) { Warp(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void Warp(Vector3Int offset)
    {
        WarpTo(GridPosition + offset);
    }

    public void WarpTo(float x, float y, float z) { WarpTo(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void WarpTo(Vector3Int point)
    {
        visual.transform.position = point;
        rb.transform.position = point;
    }

    public void MoveX(float x) { Move(x, 0, 0); }
    public void MoveY(float y) { Move(0, y, 0); }
    public void MoveZ(float z) { Move(0, 0, z); }
    public void Move(float x, float y, float z) { Move(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
    public void Move(Vector3Int offset)
    {
        MoveTo(GridPosition + offset);
    }

    public void MoveTo(float x, float y, float z) { MoveTo(Vector3Int.RoundToInt(new Vector3(x, y, z))); }
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
        Vector3[] path = new Vector3[points.Length];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = grid.ToWorldSpace(points[i]);
        }

        float calculatedDuration = duration;
        if (durationPerDistance)
            calculatedDuration *= CalculatePathDistance(path);

        visual.DOPath(path, calculatedDuration);
        rb.transform.position = points[points.Length - 1];
    }

    private float CalculatePathDistance(Vector3[] path)
    {
        float distance = 0;
        Vector3 origin = GridPosition;
        foreach (Vector3 waypoint in path)
        {
            distance += Vector3.Distance(waypoint, origin);
            origin = waypoint;
        }
        return distance;
    }
}
