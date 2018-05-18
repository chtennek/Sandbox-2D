using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void AssertSingleton<T>(this T behaviour, ref T singleton) where T : Component
    {
        if (singleton != null)
        {
            Warnings.DuplicateSingleton(behaviour);
            GameObject.Destroy(behaviour);
        }

        singleton = behaviour;
        GameObject.DontDestroyOnLoad(behaviour);
    }

    public static T GetComponentInTag<T>(this MonoBehaviour behaviour, string tag, T target = null) where T : Component
    {
        if (target == null && tag.Length > 0)
        {
            GameObject obj = GameObject.FindWithTag(tag);
            if (obj != null)
                target = obj.GetComponent<T>();
        }

        if (target == null)
            Warnings.ComponentMissing<T>(behaviour);

        return target;
    }

    public static bool Contains(this LayerMask mask, int layer)
    {
        return (mask.value & (1 << layer)) > 0;
    }

    public static Color Multiply(this Color color, float value)
    {
        return value * (Vector4)color;
    }


    public static Vector3 LargestAxis(this Vector3 vector)
    {
        if (Mathf.Abs(vector.x) >= Mathf.Abs(vector.y) && Mathf.Abs(vector.x) >= Mathf.Abs(vector.z))
            return vector.x * Vector3.right;

        if (Mathf.Abs(vector.y) >= Mathf.Abs(vector.z))
            return vector.y * Vector3.up;

        return vector.z * Vector3.forward;
    }

    public static Vector2 LargestAxis(this Vector2 vector)
    {
        return ((Vector3)vector).LargestAxis();
    }

    public static Vector3 Quantized(this Vector3 vector)
    {
        Vector3 output = vector;
        if (output.x != 0) output.x = Mathf.Sign(output.x);
        if (output.y != 0) output.y = Mathf.Sign(output.y);
        if (output.z != 0) output.z = Mathf.Sign(output.z);
        return output;
    }

    public static Vector2 Quantized(this Vector2 vector)
    {
        return ((Vector3)vector).Quantized();
    }

    public static float Sum(this Vector3 vector)
    {
        return vector.x + vector.y + vector.z;
    }

    public static float Sum(this Vector2 vector)
    {
        return vector.x + vector.y;
    }
}