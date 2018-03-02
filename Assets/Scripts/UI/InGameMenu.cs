using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Rewired;

[RequireComponent(typeof(CanvasGroup))]
public class InGameMenu : InputBehaviour, ICancelHandler
{
    [Header("Input")]
    public bool lockInput = true;
    public Selector selector;
    public Selectable firstSelected;
    public string openInputName = "UIStart"; // Use EventSystem for this to avoid dup inputs
    public string closeInputName = "UIStart";

    [Header("Options")]
    public float inactiveAlpha = 0f;

    [Header("Cancel")]
    public bool closeOnCancel = false;
    public Button invokeOnCancel;
    public UnityEvent onCancel;

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

    protected override void Awake()
    {
        base.Awake();

        if (firstSelected == null)
            firstSelected = GetComponentInChildren<Selectable>();

        canvas = GetComponent<CanvasGroup>();
        Enabled = Enabled;
    }

    private void Update()
    {
        if (input != null)
        {
            if (Enabled == false && input.GetButtonDown(openInputName))
                Open();
            else if (Enabled == true && input.GetButtonDown(closeInputName))
                Close();
        }
    }

    public void OnCancel(BaseEventData eventData)
    {
        if (eventData.used == true)
            return;

        eventData.Use();

        onCancel.Invoke();
        if (invokeOnCancel != null)
            invokeOnCancel.onClick.Invoke();

        if (closeOnCancel == true)
            Close();
    }

    public void Toggle()
    {
        if (Enabled == true)
            Close();
        else
            Open();
    }

    public void ForceOpen() { Open(true); }
    public void Open() { Open(false); }
    private void Open(bool forceLock)
    {
        if (lockInput == true)
        {
            if (forceLock == true)
                input.ForceLock();
            else
                input.Lock();

            Time.timeScale = 0;
        }
        Enabled = true;
        if (firstSelected != null)
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }

    public void Close()
    {
        if (lockInput == true)
        {
            input.Unlock();
            Time.timeScale = 1;
        }
        Enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
