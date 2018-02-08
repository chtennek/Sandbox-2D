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
        if (GUILayout.Button("Save Current"))
        {
            LevelBuilder builder = target as LevelBuilder;
            builder.SaveLevel();
        }
        if (GUILayout.Button("Load Current"))
        {
            LevelBuilder builder = target as LevelBuilder;
            builder.ReloadLevel();
        }
        DrawDefaultInspector();
    }

    private void SaveLevel()
    {

    }
}
