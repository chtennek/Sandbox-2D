using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

using Rewired;

public class InGameMenu : MonoBehaviour
{

    [Header("Input")]
    public InputReceiver input;
    public bool lockInput = true;
    public GameObject firstSelected;

    [Header("Button")]
    public string openInputName = "UIStart";
    public string closeInputName = "UIStart";
    public string cancelInputName = "UICancel";

    [Header("Cancel")]
    public bool closeOnCancel = false;
    public Button invokeOnCancel;
    public UnityEvent onCancel;

    private Mask disabledMask;

    private void Awake()
    {
        if (input == null)
        {
            Debug.LogWarning(gameObject.name + ": InputReceiver not specified! Disabling menu.");
            enabled = false;
        }

        disabledMask = GetComponent<Mask>();
        if (disabledMask == null) disabledMask = gameObject.AddComponent<Mask>();

        if (disabledMask.enabled == true) Close();
    }

    private void Update()
    {
        if (input != null)
        {
            if (disabledMask.enabled == true && input.GetButtonDown(openInputName))
            {
                Open();
            }
            else if (disabledMask.enabled == false && input.GetButtonDown(closeInputName))
            {
                Close();
            }
            else if (disabledMask.enabled == false && input.GetButtonDown(cancelInputName))
            {
                onCancel.Invoke();
                if (invokeOnCancel != null)
                {
                    invokeOnCancel.onClick.Invoke();
                }
                if (closeOnCancel == true)
                {
                    Close();
                }
            }
        }
    }

    public void Toggle()
    {
        if (disabledMask.enabled == true)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        Debug.Log("Open");
        if (lockInput == true)
        {
            input.Lock();
            Time.timeScale = 0;
        }

        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.enabled = true;
        }
        disabledMask.enabled = false;
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void Close()
    {
        Debug.Log("Close");
        if (lockInput == true)
        {
            input.Unlock();
            Time.timeScale = 1;
        }

        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.enabled = false;
        }
        disabledMask.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
