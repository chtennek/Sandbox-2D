using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryBase), true)]
public class ChildrenOverwritingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Refresh Children"))
        {
            InventoryBase inventory = target as InventoryBase;
            inventory.ResetView();
        }
        DrawDefaultInspector();
    }
}
