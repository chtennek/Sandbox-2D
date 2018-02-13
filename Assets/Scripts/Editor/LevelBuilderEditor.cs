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
            builder.Save();
        }
        if (GUILayout.Button("Load from Current"))
        {
            LevelBuilder builder = target as LevelBuilder;
            builder.Reload();
        }
        if (GUILayout.Button("Clear"))
        {
            LevelBuilder builder = target as LevelBuilder;
            builder.Clear();
        }
        GUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
