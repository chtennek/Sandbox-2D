using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySessionMessenger : MonoBehaviour
{
    public void StartGame()
    {
        PlaySessionManager.current.StartGame();
    }

    public void StartGame(int levelIndex)
    {
        PlaySessionManager.current.StartGame(levelIndex);
    }

    public void ToMainMenu()
    {
        PlaySessionManager.current.ToMainMenu();
    }

    public void ExitGame()
    {
        PlaySessionManager.current.ExitGame();
    }
}
