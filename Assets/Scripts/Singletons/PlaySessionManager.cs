using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySessionManager : MonoBehaviour
{
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

    public void LoadScene(Scene scene) { LoadScene(scene.name); }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
