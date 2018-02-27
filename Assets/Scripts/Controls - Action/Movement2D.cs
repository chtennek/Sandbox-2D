using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
public class Movement2D : MonoBehaviour
{
    public float inputDeadZone = .2f;
    public bool restrictToXAxis = true;
    public bool restrictToYAxis = true;

    [Space]

    public float walkSpeed = 5f;
    public float minWalkableSpeed = 1f;
    public float walkSpeedLevels = 2; // number of possible speeds between walkSpeed and minWalkableSpeed

    [Space]

    public float acceleration = 40; // How fast do we accelerate to walkSpeed?
    public float drag = 10; // How fast do we stop when not moving?

    [Space]

    public bool faceMovementDirection;
    public bool onlyMoveForward;
    public float rotationOffset = 90f; // At what movement direction should we be at 0 rotation?
    public float turnSpeed = Mathf.Infinity; // Degrees per frame

    private InputReceiver input;
    private Rigidbody2D rb;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get input
        Vector2 movement = (restrictToXAxis && restrictToYAxis) ? input.GetAxisPairSingle("Move") : input.GetAxisPair("Move");
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
            transform.Rotate(rotationDelta * Vector3.forward);
        }

        // Calculate target velocity
        Vector2 targetVelocity;
        float tq = Mathv.LerpQRound(0, 1, Mathf.InverseLerp(inputDeadZone, 1, movement.magnitude), walkSpeedLevels);
        if (onlyMoveForward && !isFacingMovementDirection)
        {
            targetVelocity = minWalkableSpeed * transform.TransformDirection(Quaternion.AngleAxis(rotationOffset, Vector3.forward) * Vector3.right);
        }
        else
        {
            targetVelocity = Mathf.Lerp(minWalkableSpeed, walkSpeed, tq) * movement.normalized;
        }

        // Apply acceleration
        rb.AddForce(acceleration * targetVelocity.normalized);
        rb.drag = targetVelocity.magnitude == 0 ? drag : acceleration / targetVelocity.magnitude;
    }
}
