using UnityEngine;
using System.Collections;

using Rewired;

public class InputReceiver : MonoBehaviour
{
    public int playerId = 0;
    public Player player;

    private bool jumpBuffered;
    private bool jumpReleaseBuffered;
    private Vector2 currentMovementVector;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    private void Update()
    {
        if (player.GetButtonDown("Jump"))
        {
            jumpBuffered = true;
        }
        if (player.GetButtonUp("Jump"))
        {
            jumpReleaseBuffered = true;
        }
        float x = player.GetAxisRaw("Move Horizontal");
        float y = player.GetAxisRaw("Move Vertical");
        currentMovementVector = new Vector2(x, y);
    }

    public Vector2 GetQuantizedMovementVector()
    {
        Vector2 output = currentMovementVector;
        if (output.x > 0)
        {
            output.x = 1;
        }
        else if (output.x < 0)
        {
            output.x = -1;
        }
        if (output.y > 0)
        {
            output.y = 1;
        }
        else if (output.y < 0)
        {
            output.y = -1;
        }
        return output;
    }

    public Vector2 GetCircularMovementVector()
    {
        if (currentMovementVector.x == 0 || currentMovementVector.y == 0)
            return currentMovementVector;

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
        return scale * currentMovementVector;
    }

    public Vector2 GetMovementVector()
    {
        return currentMovementVector;
    }

    public bool JumpRelease()
    {
        if (jumpReleaseBuffered)
        {
            jumpReleaseBuffered = false;
            return true;
        }
        return false;
    }

    public bool Jump()
    {
        if (jumpBuffered)
        {
            jumpBuffered = false;
            return true;
        }
        return false;
    }
}
