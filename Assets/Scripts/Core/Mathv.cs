using UnityEngine;

public static class Mathv
{
    public static int ToBase10(int[] digits, int b)
    {
        // digits should be low to high
        int value = 0;
        for (int i = 0; i < digits.Length; i++)
        {
            value += digits[i] * (int)Mathf.Pow(b, i);
        }
        return value;
    }

    public static int DigitInBase(int n, int b, int i)
    {
        // i = left-shifts from one's place
        return (n - n % (int)Mathf.Pow(b, i)) % (int)Mathf.Pow(b, i + 1);
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

    // Clamp modulo value to [0, m)
    public static float Mod(float n, float m)
    {
        return (n % m + m) % m;
    }

    // Convert angle to [-180, 180) range
    public static float ClampAngle180(float angle)
    {
        return Mod(angle, 360) - 180;
    }

    public static Vector2 Car2Pol(Vector2 v)
    {
        return new Vector2(v.magnitude, Mathf.Atan2(v.y, v.x));
    }

    public static Vector2 Pol2Car(Vector2 v)
    {
        return new Vector2(v.x * Mathf.Cos(Mathf.Deg2Rad * v.y), v.x * Mathf.Sin(Mathf.Deg2Rad * v.y));
    }
}
