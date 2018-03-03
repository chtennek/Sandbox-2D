using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigator
{
    void MenuClose(InGameMenu menu);
    void MenuCloseActive();
    void MenuOpen(InGameMenu menu);
    void MenuSwitch(InGameMenu menu);
    void MenuUpOneLevel();
}
