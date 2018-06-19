using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid Settings", menuName = "Grid Settings", order = 0)]
public class GridSettings : ScriptableObject
{
    public Vector3 scale = Vector3.one;
    public Vector3 offset = Vector3.zero;

    public Vector3Int ToGridSpace(Vector3 position)
    {
        Vector3 inverseGridSize = new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
        return Vector3Int.RoundToInt(Vector3.Scale(inverseGridSize, position - offset));
    }

    public Vector3 ToWorldSpace(Vector3Int coordinates)
    {
        return Vector3.Scale(coordinates, scale) + offset;
    }
}
