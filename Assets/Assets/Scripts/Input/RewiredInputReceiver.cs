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

    public override bool GetButtonDownRaw(string id) { return player.GetButtonDown(id); }
    public override bool GetButtonUpRaw(string id) { return player.GetButtonUp(id); }
    public override bool GetButtonRaw(string id) { return player.GetButton(id); }
    public override bool GetAnyButtonDownRaw() { return player.GetAnyButtonDown(); }
    public override bool GetAnyButtonRaw() { return player.GetAnyButton(); }
    public override float GetAxisRaw(string id) { return player.GetAxisRaw(id); }
    public override bool GetPositiveAxisDown(string id) { return player.GetButtonDown(id); }
    public override bool GetNegativeAxisDown(string id) { return player.GetNegativeButtonDown(id); }

}
