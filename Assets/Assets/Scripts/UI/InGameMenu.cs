using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class InGameMenu : MonoBehaviour
{
    public Selectable firstSelected;
    public InGameMenu parentMenu;
    public float inactiveAlpha = 0f;

    private CanvasGroup canvas;

    public bool Enabled
    {
        get { return canvas.interactable; }
        set
        {
            canvas.interactable = value;
            canvas.alpha = Enabled ? 1f : inactiveAlpha;
        }
    }

    protected void Awake()
    {
        if (firstSelected == null)
            firstSelected = GetComponentInChildren<Selectable>();

        canvas = GetComponent<CanvasGroup>();
        Enabled = Enabled;
    }

    public void ToggleEnabled()
    {
        Enabled = !Enabled;
    }
}
