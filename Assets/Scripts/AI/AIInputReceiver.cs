using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputReceiver : InputReceiver {
    public string axisPairName = "Move";
    public Vector2 movement = Vector2.zero;

    public string HorizontalAxis { get { return axisPairName + "Horizontal"; } }
    public string VerticalAxis { get { return axisPairName + "Vertical"; } }

    public override bool GetAnyButtonDownRaw()
    {
        return false;
    }

    public override bool GetAnyButtonRaw()
    {
        return false;
    }

    public override float GetAxisRaw(string id)
    {
        if (id == HorizontalAxis) return movement.x;
        if (id == VerticalAxis) return movement.y;
        return 0;
    }

    public override bool GetButtonDownRaw(string id)
    {
        return false;
    }

    public override bool GetButtonRaw(string id)
    {
        return Mathf.Approximately(GetAxisRaw(id), 0);
    }

    public override bool GetButtonUpRaw(string id)
    {
        return false;
    }
}
