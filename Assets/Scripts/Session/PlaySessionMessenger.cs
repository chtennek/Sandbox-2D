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
        do
        {
            yield return null;
        } while (isReady == false);
        PlaySessionManager.current.StartGame();
    }

    private IEnumerator _StartGame(int levelIndex)
    {
        do
        {
            yield return null;
        } while (isReady == false);
        PlaySessionManager.current.StartGame(levelIndex);
    }

    private IEnumerator _ToMainMenu()
    {
        do
        {
            yield return null;
        } while (isReady == false);
        PlaySessionManager.current.ToMainMenu();
    }

    private IEnumerator _ExitGame()
    {
        do
        {
            yield return null;
        } while (isReady == false);
        PlaySessionManager.current.ExitGame();
    }
}
