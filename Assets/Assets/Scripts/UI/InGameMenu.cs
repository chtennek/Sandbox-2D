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

    public CanvasGroup canvas;

    public bool Enabled
    {
        get { return canvas.interactable; }
        set
        {
            canvas.interactable = value;
            canvas.alpha = Enabled ? 1f : inactiveAlpha;
        }
    }

    private void Reset()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    protected void Awake()
    {
        if (firstSelected == null)
            firstSelected = GetComponentInChildren<Selectable>();
        Enabled = Enabled;
    }

    public void ToggleEnabled()
    {
        Enabled = !Enabled;
    }
}
