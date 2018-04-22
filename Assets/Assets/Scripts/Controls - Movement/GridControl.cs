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
        if (input == null)
            input = GetComponent<InputReceiver>();

        if (pathControl == null)
            pathControl = GetComponent<PathControl>();

        if (gridMovement == null)
            gridMovement = GetComponent<GridMovement>();
    }

    private void Awake()
    {
        if (pathControl == null || gridMovement == null)
        {
            Warnings.ComponentMissing(this);
            enabled = false;
        }
    }

    protected virtual void Update()
    {
        Vector3 movement = (input == null) ? Vector2.zero : input.GetAxisPairSingle(axisPairName).normalized;
        if (input != null && requireDistinctPress == true && input.GetAxisPairDown(axisPairName) == false)
        {
            movement = Vector2.zero;
        }
        if (restrictToXAxis == true && restrictToYAxis == false) movement.y = 0;
        if (restrictToXAxis == false && restrictToYAxis == true) movement.x = 0;
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