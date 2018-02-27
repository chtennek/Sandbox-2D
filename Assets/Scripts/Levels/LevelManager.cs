using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Levels;

[RequireComponent(typeof(LevelBuilder))]
public class LevelManager : MonoBehaviour
{
    public delegate void LevelLoadHandler(Level level);
    public static event LevelLoadHandler OnLevelLoad;

    public LevelBuilder builder;

    private void Awake()
    {
        if (builder == null)
            builder = GetComponent<LevelBuilder>();

        return;
        if (OnLevelLoad != null && builder != null)
            OnLevelLoad += builder.Load;
    }

    public static void LoadLevel(Level level)
    {
        if (OnLevelLoad != null)
            OnLevelLoad(level);
    }
}
