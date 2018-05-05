using UnityEngine;

public enum TurnControlMode
{
    None,
    SetRotation,
    LookAt,
    FreeLook,
}

public class TurnControl : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public string axisPairName = "Aim";
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;

    public TurnControlMode controlMode;
    public bool lockCursor;

    public Vector2 sensitivity = new Vector2(.1f, 0); // Use negative to invert
    public Vector2 xRange = new Vector2(-180, 180);
    public bool xWrap = true;
    public Vector2 yRange = new Vector2(-90, 90);
    public bool yWrap = false;

    private Vector2 freeLook;

    [Header("Parameters")]
    public float turnSpeed = Mathf.Infinity; // Degrees per frame
    public Rotator rotator;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        Vector3 rotation = Grid.InverseSwizzle(swizzle, transform.rotation.eulerAngles);
        freeLook = new Vector2(rotation.z, rotation.x);

        if (input == null)
            Warnings.ComponentMissing(this);
    }

    private void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;
        Vector3 movement = Vector3.zero;

        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;

        switch (controlMode)
        {
            case TurnControlMode.SetRotation:
                direction = input.GetAxisPair(axisPairName);
                direction = Grid.Swizzle(swizzle, direction);
                RotateTowards(direction);
                break;
            case TurnControlMode.FreeLook:
                movement = input.GetAxisPair(axisPairName) * sensitivity;
                if (movement.magnitude > turnSpeed)
                    movement = turnSpeed * movement.normalized;

                RotateIn(movement);
                break;
            case TurnControlMode.LookAt:
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

        freeLook.x = Mathf.Clamp(freeLook.x + movement.x, xRange.x, xRange.y);
        freeLook.y = Mathf.Clamp(freeLook.y + movement.y, yRange.x, yRange.y);

        Vector3 rotation = new Vector3(freeLook.y, 0, freeLook.x);
        transform.rotation = Quaternion.Euler(Grid.Swizzle(swizzle, rotation));
        return;
    }
}
