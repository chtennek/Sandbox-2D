using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySessionManager : MonoBehaviour
{
    public static PlaySessionManager session;

    public void Awake()
    {
        if (session == null)
        {
            session = this;
        }
        else if (session != null && session != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
