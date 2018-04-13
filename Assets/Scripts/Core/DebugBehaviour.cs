using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugBehaviour : InputBehaviour
{
    private void Update()
    {
        if (input != null && input.GetButtonDown("Reload Scene"))
            ReloadScene();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
