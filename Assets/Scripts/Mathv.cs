using UnityEngine;

public class Mathv
{
    public static Vector2 Car2Pol(Vector2 v)
    {
        return new Vector2(v.magnitude, Mathf.Atan2(v.y, v.x));
    }

    public static Vector2 Pol2Car(Vector2 v)
    {
        return new Vector2(v.x * Mathf.Cos(Mathf.Deg2Rad * v.y), v.x * Mathf.Sin(Mathf.Deg2Rad * v.y));
    }
}
