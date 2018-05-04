using UnityEngine;

public enum TurnControlMode
{
    None,
    SetRotation,
    SetAngularVelocity,
    MouseFreeLook,
    MouseLookAt,
}

public class TurnControl : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public string axisPairName = "Aim";
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;
    public bool invertX = false;
    public bool invertY = false;

    public TurnControlMode controlMode;
    public float mouseSensitivity = 0.1f; // [TODO] move to InputReceiver

    [Header("Parameters")]
    public float turnSpeed = Mathf.Infinity; // Degrees per frame
    public Rotator rotator;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        if (input == null)
            Warnings.ComponentMissing(this);
    }

    private void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        if (controlMode == TurnControlMode.MouseFreeLook)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        switch (controlMode)
        {
            case TurnControlMode.SetRotation:
                direction = input.GetAxisPair(axisPairName);
                direction = Grid.Swizzle(swizzle, direction);
                RotateTowards(direction);
                break;
            case TurnControlMode.SetAngularVelocity:
                movement = input.GetAxisPair(axisPairName).Quantized() * turnSpeed;
                RotateIn(movement);
                break;
            case TurnControlMode.MouseFreeLook:
                movement = input.GetAxisPair(axisPairName) * mouseSensitivity;
                if (movement.magnitude > turnSpeed)
                    movement = turnSpeed * movement.normalized;

                RotateIn(movement);
                break;
            case TurnControlMode.MouseLookAt:
                Vector3 normal = Grid.Swizzle(swizzle, Vector3.forward);
                Plane plane = new Plane(normal, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance;
                plane.Raycast(ray, out distance);

                Vector3 target = ray.GetPoint(distance);
                direction = target - transform.position;
                RotateTowards(direction);
                break;
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = rotator.GetRotationTowards(direction);
        float angularDistance = Quaternion.Angle(transform.rotation, targetRotation);
        float t = turnSpeed / angularDistance;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
    }

    private void RotateIn(Vector3 movement)
    {
        if (movement == Vector3.zero)
            return;

        Vector3 xAxis = Grid.Swizzle(swizzle, invertX ? Vector3.back : Vector3.forward);
        Vector3 yAxis = Grid.Swizzle(swizzle, invertY ? Vector3.right : Vector3.left);
        Quaternion xRotation = Quaternion.AngleAxis(movement.x, xAxis);
        Quaternion yRotation = Quaternion.AngleAxis(movement.y, yAxis);
        Vector3 facing = Grid.Swizzle(swizzle, Vector3.up);
        Vector3 direction = yRotation * xRotation * transform.rotation * facing;
        RotateTowards(direction);
    }
}
