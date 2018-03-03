using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigatorMessenger : MonoBehaviour, INavigator
{
    public UINavigator nav;

    private void Awake()
    {
        if (nav == null)
            nav = GetComponentInParent<UINavigator>();

        if (nav == null)
            Debug.LogWarning("UINavigatorMessenger has no UINavigator set!");
    }

    public void MenuClose(InGameMenu menu)
    {
        if (nav != null)
            nav.MenuClose(menu);
    }

    public void MenuCloseActive()
    {
        if (nav != null)
            nav.MenuCloseActive();
    }

    public void MenuOpen(InGameMenu menu)
    {
        if (nav != null)
            nav.MenuOpen(menu);
    }

    public void MenuSwitch(InGameMenu menu)
    {
        if (nav != null)
            nav.MenuSwitch(menu);
    }

    public void MenuUpOneLevel()
    {
        if (nav != null)
            nav.MenuUpOneLevel();
    }
}