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

    public override Vector2 GetAxisPairRaw(string idBase)
    {
        string horizontal = idBase + " Horizontal";
        string vertical = idBase + " Vertical";
        float x = player.GetAxisRaw(horizontal);
        float y = player.GetAxisRaw(vertical);
        return new Vector2(x, y);
    }

    public override bool GetButtonDownRaw(string id) { return player.GetButtonDown(id); }
    public override bool GetButtonUpRaw(string id) { return player.GetButtonUp(id); }
    public override bool GetButtonRaw(string id) { return player.GetButton(id); }
    public override bool GetAnyButtonDownRaw() { return player.GetAnyButtonDown(); }
    public override bool GetAnyButtonRaw() { return player.GetAnyButton(); }
}
