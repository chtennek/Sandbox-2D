using UnityEngine;
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
    public bool faceMovementDirection;
    public bool onlyMoveForward;
    public float rotationOffset = 90f; // At what movement direction should we be at 0 rotation?
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
        Vector2 movement = (restrictToXAxis && restrictToYAxis) ? input.GetAxisPairSingle(axisPairName) : input.GetAxisPair(axisPairName);
        if (restrictToXAxis == true && restrictToYAxis == false) movement.y = 0;
        if (restrictToXAxis == false && restrictToYAxis == true) movement.x = 0;

        // Change rotation
        bool isFacingMovementDirection = true;
        if (faceMovementDirection && movement != Vector2.zero)
        {
            float rotationTarget = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + rotationOffset;
            float rotationDelta = Mathv.ClampAngle180(rotationTarget - transform.eulerAngles.z);
            isFacingMovementDirection = Mathf.Abs(rotationDelta) <= turnSpeed;
            rotationDelta = (rotationDelta > 0) ? Mathf.Min(rotationDelta, turnSpeed) : Mathf.Max(rotationDelta, -turnSpeed);
            transform.Rotate(Grid.Swizzle(swizzle, rotationDelta * Vector3.forward));
        }

        // Calculate target velocity
        movement = Grid.Swizzle(swizzle, movement);
        Vector2 targetVelocity;
        float tq = Mathv.LerpQRound(0, 1, Mathf.InverseLerp(input.deadZone, 1, movement.magnitude), walkSpeedLevels);
        if (onlyMoveForward && !isFacingMovementDirection)
        {
            targetVelocity = minWalkableSpeed * transform.TransformDirection(Quaternion.AngleAxis(rotationOffset, Vector3.forward) * Vector3.right);
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
        Vector2 v = mover.Velocity;
        // Only apply drag in restricted axis if we have one
        if (restrictToXAxis == true && restrictToYAxis == false) v.y = 0;
        if (restrictToXAxis == false && restrictToYAxis == true) v.x = 0;
        mover.AddForce(drag * -v);
    }
}
