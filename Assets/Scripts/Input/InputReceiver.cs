using UnityEngine;
using System.Collections;

using Rewired;

public class InputReceiver : MonoBehaviour
{
    public int playerId = 0;
    public Player player;
    public float defaultDeadZone = 0f;

    private Vector2 currentMovementVector;
    private Vector2 currentAimVector;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    private void Update()
    {
        float x = player.GetAxisRaw("Move Horizontal");
        float y = player.GetAxisRaw("Move Vertical");
        currentMovementVector = new Vector2(x, y);

        x = player.GetAxisRaw("Aim Horizontal");
        y = player.GetAxisRaw("Aim Vertical");
        currentAimVector = new Vector2(x, y);
    }

    public Vector2 GetSingleAxisMovementVector() { return GetSingleAxisMovementVector(defaultDeadZone); }
    public Vector2 GetSingleAxisMovementVector(float deadZone)
    {
        Vector2 output = GetMovementVector(deadZone);
        if (Mathf.Abs(output.x) >= Mathf.Abs(output.y))
        {
            output.y = 0;
        }
        else
        {
            output.x = 0;
        }
        return output;
    }

    public Vector2 GetQuantizedMovementVector() { return GetQuantizedMovementVector(defaultDeadZone); }
    public Vector2 GetQuantizedMovementVector(float deadZone)
    {
        Vector2 output = currentMovementVector;
        if (output.x > deadZone)
        {
            output.x = 1;
        }
        else if (output.x < -deadZone)
        {
            output.x = -1;
        }
        else
        {
            output.x = 0;
        }
        if (output.y > deadZone)
        {
            output.y = 1;
        }
        else if (output.y < -deadZone)
        {
            output.y = -1;
        }
        else
        {
            output.y = 0;
        }
        return output;
    }

    public Vector2 GetQuantizedAimVector() { return GetQuantizedAimVector(defaultDeadZone); }
    public Vector2 GetQuantizedAimVector(float deadZone)
    {
        Vector2 output = currentAimVector;
        if (output.x > deadZone)
        {
            output.x = 1;
        }
        else if (output.x < -deadZone)
        {
            output.x = -1;
        }
        else
        {
            output.x = 0;
        }
        if (output.y > deadZone)
        {
            output.y = 1;
        }
        else if (output.y < -deadZone)
        {
            output.y = -1;
        }
        else
        {
            output.y = 0;
        }
        return output;
    }

    public Vector2 GetCircularMovementVector() { return GetCircularMovementVector(defaultDeadZone); }
    public Vector2 GetCircularMovementVector(float deadZone)
    {
        if (currentMovementVector.x == 0 || currentMovementVector.y == 0)
        {
            if (currentMovementVector.magnitude < deadZone)
            {
                return Vector2.zero;
            }
            return currentMovementVector;
        }

        // Rescale input space from the unit square to the unit circle
        Vector2 circle = currentMovementVector.normalized;
        Vector2 square;
        float scale;
        if (Mathf.Abs(circle.x) > Mathf.Abs(circle.y))
        {
            square = new Vector2(circle.x / Mathf.Abs(circle.x), circle.y / Mathf.Abs(circle.x));
        }
        else
        {
            square = new Vector2(circle.x / Mathf.Abs(circle.y), circle.y / Mathf.Abs(circle.y));
        }
        scale = circle.magnitude / square.magnitude;

        Vector2 output = scale * currentMovementVector;
        if (output.magnitude < deadZone)
        {
            return Vector2.zero;
        }
        return output;
    }

    public Vector2 GetCircularAimVector() { return GetCircularAimVector(defaultDeadZone); }
    public Vector2 GetCircularAimVector(float deadZone)
    {
        if (currentAimVector.x == 0 || currentAimVector.y == 0)
        {
            if (currentAimVector.magnitude < deadZone)
            {
                return Vector2.zero;
            }
            return currentAimVector;
        }

        // Rescale input space from the unit square to the unit circle
        Vector2 circle = currentAimVector.normalized;
        Vector2 square;
        float scale;
        if (Mathf.Abs(circle.x) > Mathf.Abs(circle.y))
        {
            square = new Vector2(circle.x / Mathf.Abs(circle.x), circle.y / Mathf.Abs(circle.x));
        }
        else
        {
            square = new Vector2(circle.x / Mathf.Abs(circle.y), circle.y / Mathf.Abs(circle.y));
        }
        scale = circle.magnitude / square.magnitude;

        Vector2 output = scale * currentAimVector;
        if (output.magnitude < deadZone)
        {
            return Vector2.zero;
        }
        return output;
    }

    public Vector2 GetMovementVector() { return GetMovementVector(defaultDeadZone); }
    public Vector2 GetMovementVector(float deadZone)
    {
        if (currentMovementVector.magnitude < deadZone)
        {
            return Vector2.zero;
        }
        return currentMovementVector;
    }

    public Vector2 GetAimVector() { return GetAimVector(defaultDeadZone); }
    public Vector2 GetAimVector(float deadZone)
    {
        if (currentAimVector.magnitude < deadZone)
        {
            return Vector2.zero;
        }
        return currentAimVector;
    }

    public float GetAimRotation()
    {
        return Mathf.Atan2(currentAimVector.y, currentAimVector.x) * Mathf.Rad2Deg;
    }
}
