using UnityEngine;

public static class Mathv
{
    public static Vector3 RotateAround(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        direction = rotation * direction;
        return direction + pivot;
    }
}
