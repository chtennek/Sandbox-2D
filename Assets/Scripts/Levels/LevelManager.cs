using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Levels;

[RequireComponent(typeof(LevelBuilder))]
public class LevelManager : MonoBehaviour
{
    public delegate void LevelLoadHandler(Level level);
    public static event LevelLoadHandler OnLevelLoad;

    [SerializeField]
    private LevelBuilder builder;

    private void Awake()
    {
        if (builder != null)
            builder = GetComponent<LevelBuilder>();

        if (OnLevelLoad != null)
            OnLevelLoad += builder.Load;
    }

    public static void LoadLevel(Level level)
    {
        if (OnLevelLoad != null)
            OnLevelLoad(level);
    }
}
