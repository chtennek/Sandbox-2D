using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineRendererFamily : MonoBehaviour
{
    public Vector3[] positions;
    private LineRenderer[] lines;

    private void Start()
    {

        lines = GetComponentsInChildren<LineRenderer>();
    }

    void Update()
    {
        SetPositions(positions);
    }

    public void SetPositions(Vector3[] positions)
    {
        foreach (LineRenderer line in lines)
        {
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }
}
