using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySessionMessenger : MonoBehaviour
{
    public bool isReady = true;

    public void StartGame() { StartCoroutine("_StartGame"); }
    public void StartGame(int levelIndex) { StartCoroutine("_StartGame", levelIndex); }
    public void ToMainMenu() { StartCoroutine("_ToMainMenu"); }
    public void ExitGame() { StartCoroutine("_ExitGame"); }

    private IEnumerator _StartGame()
    {
        yield return null;
        while (isReady == false)
        {
            yield return null;
        }
        PlaySessionManager.current.StartGame();
    }

    private IEnumerator _StartGame(int levelIndex)
    {
        yield return null;
        while (isReady == false)
        {
            yield return null;
        }
        PlaySessionManager.current.StartGame(levelIndex);
    }

    private IEnumerator _ToMainMenu()
    {
        yield return null;
        while (isReady == false)
        {
            yield return null;
        }
        PlaySessionManager.current.ToMainMenu();
    }

    private IEnumerator _ExitGame()
    {
        yield return null;
        while (isReady == false)
        {
            yield return null;
        }
        PlaySessionManager.current.ExitGame();
    }
}
