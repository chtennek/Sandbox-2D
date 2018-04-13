using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
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
}