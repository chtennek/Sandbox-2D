using UnityEngine;

public static class Mathv
{
    public static Vector3 RotateAround(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        direction = rotation * direction;
        return direction + pivot;
    }

    // Lerp, but round to the nearest of n evenly spaced points in [a, b]
    public static float LerpQRound(float a, float b, float t, float n)
    {
        if (n <= 1)
        {
            return (a + b) / 2;
        }
        else if (n == Mathf.Infinity)
        {
            return Mathf.Lerp(a, b, t);
        }
        float tq = Mathf.Round(t * (n - 1)) / (n - 1);
        return Mathf.Lerp(a, b, tq);
    }
}
