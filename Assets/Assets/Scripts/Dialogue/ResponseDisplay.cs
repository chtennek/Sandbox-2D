using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseDisplay : MonoBehaviour, IDisplayer<string> {
    public Text response;
    public Button button;

    public void Display(string data)
    {
        if (response != null)
            response.text = data;

        if (button != null && DialogueBehaviour.main != null)
            button.onClick.AddListener(delegate { DialogueBehaviour.main.LoadDialogueBranch(data); });
    }
}
