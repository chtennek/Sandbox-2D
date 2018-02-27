using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMessenger : MonoBehaviour
{
    public bool isReady = true; // [TODO] find a less hackish way to do this

    public void StartGame(bool startNewGame) { StartCoroutine("_StartGame", startNewGame); }
    public void ToMainMenu() { StartCoroutine("_ToMainMenu"); }
    public void ExitGame() { StartCoroutine("_ExitGame"); }

    public void SaveGame() { DataSerializer.Save(); }
    public void LoadGame() { DataSerializer.Load(); }

    private IEnumerator _StartGame(bool startNewGame)
    {
        do
        {
            yield return null;
        } while (isReady == false);
        PlaySessionManager.StartGame(startNewGame);
    }

    private IEnumerator _ToMainMenu()
    {
        do
        {
            yield return null;
        } while (isReady == false);
        PlaySessionManager.ToMainMenu();
    }

    private IEnumerator _ExitGame()
    {
        do
        {
            yield return null;
        } while (isReady == false);
        PlaySessionManager.ExitGame();
    }
}
