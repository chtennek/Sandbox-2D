﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementManager))]
public class MoveControl : InputBehaviour
{
    [Header("Input")]
    public string axisPairName = "Move";
    public bool restrictToXAxis = true;
    public bool restrictToYAxis = true;
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;

    [Header("Speed")]
    public float walkSpeed = 5f;
    public float minWalkableSpeed = 5f;
    public float walkSpeedLevels = 2; // number of possible speeds between walkSpeed and minWalkableSpeed

    [Header("Acceleration")]
    public float acceleration = 40; // How fast do we accelerate to walkSpeed?
    public float deceleration = 10; // How fast do we stop when not moving?

    [Header("Rotation")]
    //public Vector3 defaultForwardDirection = Vector3.up; // At Quaternion.identity, what direction is forward?
    public bool faceMovementDirection;
    public bool onlyMoveForward;
    public bool lookWithYAxis = false;
    public Vector3 constantAxis = Vector3.up;
    public float turnSpeed = Mathf.Infinity; // Degrees per frame

    private MovementManager mover;

    protected override void Awake()
    {
        base.Awake();
        mover = GetComponent<MovementManager>();
    }

    protected void FixedUpdate()
    {
        // Get input
        Vector3 movement = (restrictToXAxis && restrictToYAxis) ? input.GetAxisPairSingle(axisPairName) : input.GetAxisPair(axisPairName);
        movement = Grid.Swizzle(swizzle, movement);
        if (restrictToXAxis == true && restrictToYAxis == false) movement.y = 0;
        if (restrictToXAxis == false && restrictToYAxis == true) movement.x = 0;

        // Change rotation
        bool isFacingMovementDirection = true;
        if (faceMovementDirection && movement != Vector3.zero)
        {
            Quaternion targetRotation = lookWithYAxis ? Quaternion.LookRotation(constantAxis, movement) : Quaternion.LookRotation(movement, constantAxis);
            float rotationDelta = Quaternion.Angle(transform.rotation, targetRotation);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed / rotationDelta);
            isFacingMovementDirection = rotationDelta <= turnSpeed;
        }

        // Figure out which walk speed to use
        float tq = Mathv.LerpQRound(0, 1, Mathf.InverseLerp(input.deadZone, 1, movement.magnitude), walkSpeedLevels);

        // Calculate target velocity
        Vector3 targetVelocity;
        if (onlyMoveForward && !isFacingMovementDirection)
        {
            // Turning around, move at min speed
            Vector3 forward = transform.rotation * (lookWithYAxis ? Vector3.up : Vector3.forward);
            targetVelocity = minWalkableSpeed * forward;
        }
        else
        {
            targetVelocity = Mathf.Lerp(minWalkableSpeed, Mathf.Max(walkSpeed, minWalkableSpeed), tq) * movement.normalized;
        }

        if (acceleration == Mathf.Infinity)
            mover.Velocity = targetVelocity;
        else
        {
            float drag = targetVelocity.magnitude == 0 ? deceleration : acceleration / targetVelocity.magnitude;
            ApplyDrag(drag);
            mover.AddForce(acceleration * targetVelocity.normalized);
        }
    }

    private void ApplyDrag(float drag)
    {
        Vector3 v = Grid.InverseSwizzle(swizzle, mover.Velocity);
        // Only apply drag in restricted axis if we have one
        if (restrictToXAxis == true && restrictToYAxis == false) v.y = 0;
        if (restrictToXAxis == false && restrictToYAxis == true) v.x = 0;
        mover.AddForce(drag * -Grid.Swizzle(swizzle, v));
    }
}
