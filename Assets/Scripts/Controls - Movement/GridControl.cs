using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class GridControl : InputBehaviour
{
    [Header("Input")]
    public string axisPairName = "Move";
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;
    public float bufferWindow = 0f; // Set to negative for input delay

    [Header("Behaviours")]
    public bool turnSeparately = false;
    public Vector3 gravity = .1f * Vector3.down; // "gravity" by [TODO] travelTime per unit
    public bool slide; // [TODO] ice behaviour

    private PathControl pathControl;
    private GridMovement gridMovement;

    protected override void Awake()
    {
        base.Awake();
        pathControl = GetComponent<PathControl>();
        gridMovement = GetComponent<GridMovement>();
    }

    public void SnapToGrid() {
        gridMovement.Move(Vector3.zero);
    }

    private void Update()
    {
        Vector3 movement = (input == null) ? Vector2.zero : input.GetAxisPairSingle(axisPairName).normalized;
        movement = Grid.Swizzle(swizzle, movement);

        // Process gravity first
        if (gravity != Vector3.zero) // [TODO] improve performance, cache check?
        {
            Vector3 direction = Vector3.Scale(gridMovement.gridScale, gravity.normalized);
            if (gridMovement.Move(direction, true) == true)
                return;
        }

        if (Time.time < pathControl.EndTime - bufferWindow)
            return;

        // Process movement
        if (movement == Vector3.zero)
            return;

        if (turnSeparately == true)
        {
            if (gridMovement.RotateTowards(movement, Vector3.zero) == true)
                return;
        }

        gridMovement.Move(movement);
    }
}