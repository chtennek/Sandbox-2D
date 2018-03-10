using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointControl))]
public class PathEditor : Editor
{
    private WaypointControl t;

    private void OnEnable()
    {
        EditorApplication.update += Update;
        t = target as WaypointControl;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    private void Update()
    {
        if (t == null) return;
        Repaint();
    }

    private void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck(); // [TODO] Move to Editor script
        DrawPath();
        DrawHandles();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Handle Position");
        }
    }

    private void DrawPath()
    {
        for (int i = 0; i < t.initialPoints.Length; i++)
        {
            Waypoint i0 = (i == 0) ? null : t.initialPoints[i - 1];
            Waypoint i1 = t.initialPoints[i];
            Vector3 v0 = t.transform.TransformPoint((i == 0) ? Vector3.zero : i0.localPosition);
            Vector3 v1 = t.transform.TransformPoint(i1.localPosition);
            float approachRadius = new Vector2(i1.approachCurvature, (v1 - v0).magnitude / 2).magnitude;

            Handles.matrix = Matrix4x4.TRS(v1, Quaternion.identity, Vector3.one);
            Handles.matrix = Matrix4x4.identity;
            Handles.color = Color.green;
            if (i1.approachCurvature != 0)
            {
                Vector3 arcCenter = (v0 + v1) / 2 + Vector3.Cross(v1 - v0, Vector3.forward).normalized * i1.approachCurvature;
                float theta = Vector3.Dot(v0 - arcCenter, v1 - arcCenter) / ((v0 - arcCenter).magnitude * (v1 - arcCenter).magnitude);
                Handles.DrawWireArc(Vector3.zero, Vector3.forward, Vector3.up, 90, 1);
                Handles.DrawWireArc(arcCenter, Vector3.forward, v1 - arcCenter, theta, approachRadius);
                //Gizmos.DrawWireSphere(arcCenter, approachRadius);
            }
            else
            {
                Handles.DrawLine(v0, v1);
            }
        }
    }

    private void DrawHandles()
    {
        foreach (Waypoint w in t.initialPoints)
        {
            w.localPosition = t.transform.InverseTransformPoint(Handles.FreeMoveHandle(t.transform.TransformPoint(w.localPosition), Quaternion.identity, .25f, Vector3.one, Handles.RectangleHandleCap));
        }
    }
}
