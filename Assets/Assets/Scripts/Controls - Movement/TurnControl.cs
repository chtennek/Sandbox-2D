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

    public TurnControlMode mouseMode;
    public float mouseSensitivity; // [TODO] move to InputReceiver

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
        Vector3 direction, movement;
        if (mouseMode == TurnControlMode.MouseFreeLook)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        switch (mouseMode)
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
                movement = Input.mouseScrollDelta * mouseSensitivity;
                if (movement.magnitude > turnSpeed)
                    movement = turnSpeed * movement.normalized;

                RotateIn(movement);
                break;
            case TurnControlMode.MouseLookAt:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                direction = target - transform.position;
                Debug.Log(Input.mousePosition);
                Debug.Log(target);
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

        transform.rotation = Quaternion.AngleAxis(movement.x, transform.rotation * Grid.Swizzle(swizzle, Vector3.forward)) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(movement.y, transform.rotation * Grid.Swizzle(swizzle, Vector3.right)) * transform.rotation;
    }
}
