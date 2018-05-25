using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigator
{
    void MenuClose(GameMenu menu);
    void MenuCloseActive();
    void MenuOpen(GameMenu menu);
    void MenuSwitch(GameMenu menu);
    void MenuUpOneLevel();
}
