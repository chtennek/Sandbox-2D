using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class GridControl : InputBehaviour
{
    [Header("Input")]
    public string axisPairName = "Move";
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;
    public bool turnSeparately = true;

    private PathControl pathControl;
    private GridMovement gridMovement;

    protected override void Awake()
    {
        base.Awake();
        pathControl = GetComponent<PathControl>();
        gridMovement = GetComponent<GridMovement>();
    }

    private void Update()
    {
        // [TODO] Check if we are close enough to buffer the next movement

        Vector3 movement = input.GetAxisPairSingle(axisPairName).normalized;
        movement = Grid.Swizzle(swizzle, movement);
        if (movement == Vector3.zero)
            return;

        if (turnSeparately == false || gridMovement.RotateTowards(movement, Vector3.zero) == false)
            gridMovement.Move(movement);
    }
}