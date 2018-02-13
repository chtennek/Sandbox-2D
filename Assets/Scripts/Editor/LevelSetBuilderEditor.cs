using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Levels;

[CustomEditor(typeof(LevelSetBuilder), true)]
public class LevelSetBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Quick Save"))
        {
            LevelSetBuilder builder = target as LevelSetBuilder;
            builder.Save();
        }
        if (GUILayout.Button("Save All"))
        {
            LevelSetBuilder builder = target as LevelSetBuilder;
            builder.SaveChunks();
            builder.Save();
        }
        if (GUILayout.Button("Load All"))
        {
            LevelSetBuilder builder = target as LevelSetBuilder;
            builder.Reload();
        }
        if (GUILayout.Button("Clear"))
        {
            LevelSetBuilder builder = target as LevelSetBuilder;
            builder.Clear();
        }
        GUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}
