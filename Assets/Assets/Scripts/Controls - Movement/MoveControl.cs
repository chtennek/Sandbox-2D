using UnityEngine;

[RequireComponent(typeof(RigidbodyWrapper))]
public class MoveControl : MonoBehaviour
{
    public RigidbodyWrapper mover;

    [Header("Input")]
    public InputReceiver input;
    public string axisPairName = "Move";
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;
    public bool relativeToRotation;

    [Header("Velocity")]
    public float minSpeed = 5f;
    public float maxSpeed = 5f;
    public AnimationCurve speedMapping = AnimationCurve.Linear(0, 0, 1, 1);
    public float acceleration = 40; // How fast do we accelerate to walkSpeed?
    public float deceleration = 10; // How fast do we stop when not moving?

    [Header("Rotation")]
    public bool faceMovementDirection;
    public bool onlyMoveForward; // Turn on to force minimal movement when turning
    public Rotator rotator;
    public float turnSpeed = Mathf.Infinity; // Degrees per frame

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
        mover = GetComponent<RigidbodyWrapper>();
    }

    private void Awake()
    {
        if (input == null || mover == null)
            Warnings.ComponentMissing(this);
    }

    protected void FixedUpdate()
    {
        if (input == null || mover == null)
            return;

        // Get input
        Vector3 movement = input.GetAxisPair(axisPairName);
        movement = Grid.Swizzle(swizzle, movement);
        if (relativeToRotation)
            movement = transform.rotation * movement;

        // Change rotation
        bool isFacingMovementDirection = true;
        if (faceMovementDirection && movement != Vector3.zero)
        {
            Quaternion targetRotation = rotator.GetRotationTowards(movement);
            float rotationDelta = Quaternion.Angle(transform.rotation, targetRotation);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed / rotationDelta);
            isFacingMovementDirection = rotationDelta <= turnSpeed;
        }

        // Figure out which walk speed to use

        // Calculate target velocity
        Vector3 targetVelocity;
        if (onlyMoveForward && !isFacingMovementDirection)
        {
            // Turning around, move at min speed
            Vector3 forward = transform.rotation * (rotator.lookWithYAxis ? Vector3.up : Vector3.forward);
            targetVelocity = minSpeed * forward;
        }
        else
        {
            float t = Mathf.InverseLerp(input.deadZone, 1, movement.magnitude);
            targetVelocity = Mathf.Lerp(minSpeed, maxSpeed, t) * movement.normalized;
        }

        // Apply required force for target velocity
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

        if (input.restrictToXAxis && input.restrictToYAxis)
            v = v.LargestAxis();
        else if (input.restrictToXAxis)
            v.y = 0;
        else if (input.restrictToYAxis)
            v.x = 0;

        v.z = 0; // Only apply drag along movement plane
        mover.AddForce(drag * -Grid.Swizzle(swizzle, v));
    }
}
