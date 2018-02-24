using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Levels;

public class PlaySessionManager : MonoBehaviour
{
    public string mainMenuScene;
    public string gameScene;

    public List<Level> levels = new List<Level>();
    private Level currentLevel = null;

    #region Singleton pattern
    private static PlaySessionManager _current;
    public static PlaySessionManager current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType<PlaySessionManager>();

                if (_current == null)
                {
                    GameObject container = new GameObject("Singleton Container");
                    _current = container.AddComponent<PlaySessionManager>();
                }
            }

            return _current;
        }
    }

    private bool EnsureSingleton()
    {
        if (_current == null)
        {
            _current = this;
        }
        else if (_current != null && _current != this)
        {
            Destroy(gameObject);
            return false;
        }
        DontDestroyOnLoad(gameObject);
        return true;
    }
    #endregion

    public void Awake()
    {
        if (EnsureSingleton() == false) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameScene)
        {
            LevelManager.LoadLevel(currentLevel);
        }
    }

    public void StartGame()
    {
        currentLevel = null;
        SceneManager.LoadScene(gameScene);
    }

    public void StartGame(int levelIndex)
    {
        currentLevel = levels[levelIndex];
        SceneManager.LoadScene(gameScene);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
