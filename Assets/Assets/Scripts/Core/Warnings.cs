using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Warnings
{
    public static void NoSingleton<T>()
    {
        string message = typeof(T).Name + ": No singleton in scene!";
        Debug.LogWarning(message);
    }

    public static void DuplicateSingleton(Component c)
    {
        string message = c.name + "." + c.GetType().Name + ": More than one singleton in scene!";
        Debug.LogWarning(message);
    }

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

    public static void Log(Component c, string msg)
    {
        string message = c.name + "." + c.GetType().Name + ": " + msg;
        Debug.LogWarning(message);
    }
}
