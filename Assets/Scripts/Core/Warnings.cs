using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Warnings
{
    public static string RequireComponent(MonoBehaviour m)
    {
        return m.name + "." + m.GetType().Name + ": Missing required components!";
    }

    public static string NoComponentSpecified(MonoBehaviour m, Component c)
    {
        return m.name + "." + m.GetType().Name + ": No " + c.GetType().Name + " specified!";
    }
}
