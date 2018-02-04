using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySessionManager : MonoBehaviour
{
    public static PlaySessionManager current;

    public void Awake()
    {
        // Establish singleton
        if (current == null)
            current = this;
        else if (current != null && current != this)
            Destroy(gameObject);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
