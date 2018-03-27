using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathControl))]
public class GridMoveControl : InputBehaviour
{
    [Header("Input")]
    public string axisPairName = "Move";
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;

    [Header("Parameters")]
    public Vector3 gridScale = Vector3.one;
    public Vector3 gridOffset = Vector3.zero;
    public float travelTime = .1f;
    public float bufferWindow = 0.1f; // percentage of grid space length
    public LayerMask wallColliderMask;

    private PathControl pathControl;

    protected override void Awake()
    {
        base.Awake();
        pathControl = GetComponent<PathControl>();
    }

    private void Update()
    {
        // Check if we are close enough to buffer the next movement
        if (pathControl.Count < 1)
        {
            Vector3 movement = input.GetAxisPairSingle(axisPairName).normalized;
            movement = Grid.Swizzle(swizzle, movement);

            // Check if there's input or not
            if (movement != Vector3.zero)
            {
                Vector3 target = FindNearestGridPoint(transform.position + Vector3.Scale(gridScale, movement));

                // Check if there's something in the way
                if (Physics.CheckSphere(target, 0, wallColliderMask) == false && Physics2D.OverlapPoint(target, wallColliderMask) == null)
                {
                    pathControl.AddWaypoint(target, 1 / travelTime);
                }
            }
        }
    }

    private Vector3 FindNearestGridPoint(Vector3 position)
    {
        Vector3 inverseGridSize = new Vector3(1 / gridScale.x, 1 / gridScale.y, 1 / gridScale.z);
        Vector3 normalizedPosition = Vector3.Scale(inverseGridSize, position - gridOffset);
        normalizedPosition.x = Mathf.Round(normalizedPosition.x);
        normalizedPosition.y = Mathf.Round(normalizedPosition.y);
        normalizedPosition.z = Mathf.Round(normalizedPosition.z);
        return Vector3.Scale(gridScale, normalizedPosition) + gridOffset;
    }
}
