using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugBehaviour : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public string buttonName = "Reload Scene";

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        if (input == null)
            Warnings.ComponentMissing(this);
    }

    private void Update()
    {
        if (input != null && input.GetButtonDown(buttonName))
            ReloadScene();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
