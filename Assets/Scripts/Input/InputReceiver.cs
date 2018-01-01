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

    public Vector3 GetQuantizedMovementVector()
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

    public Vector3 GetMovementVector()
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
