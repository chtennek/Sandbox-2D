using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MenuButton : Button
{
    private GameMenu menu;
    public GameMenu Menu { get { return menu; } }

    protected override void Awake()
    {
        base.Awake();
        base.OnCanvasGroupChanged();
        menu = GetComponentInParent<GameMenu>();
    }

    protected override void OnTransformParentChanged()
    {
        base.OnTransformParentChanged();
        base.OnCanvasGroupChanged();
        menu = GetComponentInParent<GameMenu>();
    }
}
