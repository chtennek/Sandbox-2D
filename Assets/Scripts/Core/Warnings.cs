using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Warnings
{
    public static string ComponentMissing(MonoBehaviour m)
    {
        return m.name + "." + m.GetType().Name + ": Missing required components!";
    }

    public static string NoComponentSpecified(MonoBehaviour m, System.Type t)
    {
        return m.name + "." + m.GetType().Name + ": No " + t.ToString() + " specified!";
    }
}
