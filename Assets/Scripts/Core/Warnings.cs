using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Warnings
{
    public static void ComponentMissing(Component c)
    {
        string message = c.name + "." + c.GetType().Name + ": Missing required components!";
        Debug.LogWarning(message);
    }

    public static void ComponentMissing<T>(Component c)
    {
        string message = c.name + "." + c.GetType().Name + ": Missing " + typeof(T).ToString() + "!";
        Debug.LogWarning(message);
    }
}
