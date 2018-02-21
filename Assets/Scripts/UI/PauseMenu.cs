using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Rewired;

public class PauseMenu : MonoBehaviour {
    public string buttonName = "UIStart";
    public InputReceiver input;

    public GameObject firstSelected;

    private Mask inactiveMask;

    public void Awake()
    {
        if (input == null) enabled = false;

        inactiveMask = GetComponent<Mask>();
        if (inactiveMask == null) inactiveMask = gameObject.AddComponent<Mask>();
    }

	void Update () {
        if (input != null && input.GetButtonDown("UIStart")) {
            TogglePause();
        }
	}

    private void TogglePause() {
        if (inactiveMask.enabled == true) {
            Pause();
        }
        else {
            Unpause();
        }
    }

    private void Pause() {
        input.Lock();
        Time.timeScale = 0;
        inactiveMask.enabled = false;

        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    private void Unpause() {
        input.Unlock();
        Time.timeScale = 1;
        inactiveMask.enabled = true;
    }
}
