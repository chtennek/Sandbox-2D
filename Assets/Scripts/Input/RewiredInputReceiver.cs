using UnityEngine;
using System.Collections;

using Rewired;

public class RewiredInputReceiver : InputReceiver
{
    public Player player;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    public override Vector2 PollMovementVector()
    {
        float x = player.GetAxisRaw("Move Horizontal");
        float y = player.GetAxisRaw("Move Vertical");
        return new Vector2(x, y);
    }

    public override Vector2 PollAimVector()
    {
        float x = player.GetAxisRaw("Aim Horizontal");
        float y = player.GetAxisRaw("Aim Vertical");
        return new Vector2(x, y);
    }

    public override bool GetButtonDown(string id) { return player.GetButtonDown(id); }
    public override bool GetButtonUp(string id) { return player.GetButtonUp(id); }
    public override bool GetButton(string id) { return player.GetButton(id); }
}
