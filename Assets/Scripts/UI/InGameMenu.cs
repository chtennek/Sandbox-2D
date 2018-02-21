using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Rewired;

public class InGameMenu : MonoBehaviour
{
    public string buttonName = "UIStart";
    public InputReceiver input;

    public bool lockInput = true;
    public bool buttonClosesMenu = true;
    public GameObject firstSelected;

    private Mask inactiveMask;

    public void Awake()
    {
        if (input == null) enabled = false;

        inactiveMask = GetComponent<Mask>();
        if (inactiveMask == null) inactiveMask = gameObject.AddComponent<Mask>();
    }

    void Update()
    {
        if (input != null && input.GetButtonDown(buttonName))
        {
            if (buttonClosesMenu == true)
            {
                Toggle();
            }
            else
            {
                Open();
            }
        }
    }

    private void Toggle()
    {
        if (inactiveMask.enabled == true)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void Open()
    {
        if (inactiveMask.enabled == false)
        {
            return;
        }
        if (lockInput == true)
        {
            input.Lock();
            Time.timeScale = 0;
        }
        inactiveMask.enabled = false;
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    private void Close()
    {
        if (inactiveMask.enabled == true)
        {
            return;
        }
        if (lockInput == true)
        {
            input.Unlock();
            Time.timeScale = 1;
        }
        inactiveMask.enabled = true;
    }
}
