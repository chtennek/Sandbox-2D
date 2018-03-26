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
    public float gridSize = 1f;
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
            Vector2 movement = input.GetAxisPairSingle(axisPairName).normalized;

            // Check if there's input or not
            if (movement != Vector2.zero)
            {
                Vector3 target = transform.position + gridSize * (Vector3)movement;

                // Check if there's something in the way
                if (Physics2D.OverlapPoint(target, wallColliderMask) == null)
                {
                    pathControl.AddWaypoint(target, 1 / travelTime);
                }
            }
        }
    }
}
