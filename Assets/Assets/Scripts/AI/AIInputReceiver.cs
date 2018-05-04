using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputReceiver : InputReceiver
{
    public string axisPairName = "Move";
    public Vector2 movement = Vector2.zero; // [TODO] handle other axes
    public HashSet<string> buttonsHeld = new HashSet<string>();

    [SerializeField]
    private string[] m_buttonsHeld = new string[0];
    private string[] m_buttonsPressed = new string[0]; // [TODO] event consuming system extendable to InputReceiver

    public string HorizontalAxis { get { return axisPairName + "Horizontal"; } }
    public string VerticalAxis { get { return axisPairName + "Vertical"; } }

    private void Awake()
    {
        foreach (string buttonName in m_buttonsHeld)
            buttonsHeld.Add(buttonName);
    }

    public override bool GetAnyButtonDownRaw()
    {
        return false;
    }

    public override bool GetAnyButtonRaw()
    {
        return buttonsHeld.Count > 0;
    }

    public override float GetAxisRaw(string id)
    {
        if (id == HorizontalAxis) return movement.x;
        if (id == VerticalAxis) return movement.y;
        return 0;
    }

    public override float GetAxisDownRaw(string id)
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
        return buttonsHeld.Contains(id) || (Mathf.Approximately(GetAxisRaw(id), 0) == false);
    }

    public override bool GetButtonUpRaw(string id)
    {
        return false;
    }
}
