using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Levels;

[CustomEditor(typeof(LevelBuilder), true)]
public class LevelBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save to Current"))
        {
            LevelBuilder builder = target as LevelBuilder;
            builder.SaveLevel();
        }
        if (GUILayout.Button("Load from Current"))
        {
            LevelBuilder builder = target as LevelBuilder;
            builder.ReloadLevel();
        }
        if (GUILayout.Button("Clear Level"))
        {
            LevelBuilder builder = target as LevelBuilder;
            builder.currentLevel = null;
            builder.ClearLevel();
        }
        GUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
