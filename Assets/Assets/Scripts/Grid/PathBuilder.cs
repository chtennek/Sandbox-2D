using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool useMouse;
    [SerializeField] private CompoundMask mask;
    [SerializeField] private int maxUndoLength = 4; // 4 will undo a U shaped bend
    [SerializeField] private int maxLength = 10;
    [SerializeField] private Grid.CellSwizzle swizzle;

    [Header("References")]
    [SerializeField] private LineRenderer renderer;
    [SerializeField] private GridEntity entity;

    private List<Vector3Int> points = new List<Vector3Int>();

    private void Reset()
    {
        renderer = GetComponent<LineRenderer>();
        entity = GetComponent<GridEntity>();
    }

    private void Update()
    {
        if (useMouse)
        {
            List<Vector3> targets = mask.Mousecast();
            if (targets.Count > 0)
            {
                Vector3 target = targets[0];
                Vector3Int point = entity.grid.ToGridSpace(target);
                BuildPathAt(point);
            }
        }
    }

    public void BuildPathAt(int x, int y, int z) { BuildPathAt(new Vector3Int(x, y, z)); }
    public void BuildPathAt(Vector3Int point)
    {
        int indexOf = points.LastIndexOf(point);
        if (indexOf == -1)
            Append(point);
        else if (indexOf + maxUndoLength >= points.Count)
            RemoveAllAfter(indexOf);
    }

    public void Clear()
    {
        points.Clear();
        Refresh();
    }

    public void RemoveAllAfter(int index)
    {
        points.RemoveRange(index + 1, points.Count - (index + 1));
        Refresh();
    }

    public void Append(Vector3Int point)
    {
        if (points.Count == maxLength + 1) // Add one for starting point
            return;

        points.Add(point);
        Refresh();
    }

    public void Refresh()
    {
        Vector3[] waypoints = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++)
            waypoints[i] = Grid.InverseSwizzle(swizzle, entity.grid.ToWorldSpace(points[i]));
        renderer.positionCount = waypoints.Length;
        renderer.SetPositions(waypoints);
    }
}
