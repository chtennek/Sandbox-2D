using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class GridControl : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public string axisPairName = "Move";
    public bool restrictToXAxis = true;
    public bool restrictToYAxis = true;
    public bool requireDistinctPress = false;
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;
    public float bufferWindow = 0f; // Set to negative for input delay

    [Header("Behaviours")]
    public bool turnSeparately = false;
    public Vector3 gravity = .1f * Vector3.down; // "gravity" by [TODO] travelTime per unit
    public bool slide; // [TODO] ice behaviour

    [Header("References")]
    [SerializeField]
    private PathControl pathControl;
    [SerializeField]
    private GridMovement gridMovement;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
        pathControl = GetComponent<PathControl>();
        gridMovement = GetComponent<GridMovement>();
    }

    private void Awake()
    {
        if (pathControl == null || gridMovement == null)
            Warnings.ComponentMissing(this);
    }

    protected virtual void Update()
    {
        Vector3 movement = Vector3.zero;
        if (input != null)
        {
            if (requireDistinctPress)
                movement = input.GetAxisPairDown(axisPairName).LargestAxis().normalized;
            else
                movement = input.GetAxisPair(axisPairName).LargestAxis().normalized;
        }

        if (restrictToXAxis && restrictToYAxis)
            movement = movement.LargestAxis();
        else if (restrictToXAxis)
            movement.y = 0;
        else if (restrictToYAxis)
            movement.x = 0;
        movement = Grid.Swizzle(swizzle, movement); // [TODO] merge code with MoveControl

        // Process gravity first
        if (gravity != Vector3.zero) // [TODO] improve performance, cache check?
        {
            Vector3 direction = Vector3.Scale(gridMovement.gridScale, gravity.normalized);
            if (gridMovement.Move(direction, true) == true)
                return;
        }

        if (Time.time < pathControl.EndTime - bufferWindow)
            return;

        ProcessMovement(movement);
    }

    protected virtual void ProcessMovement(Vector3 movement)
    {
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