using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class ScreenDialogueBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("Input")]
    public InputReceiver input;
    public List<string> advanceOnInputNames;

    [Header("References")]
    public Text actorText;
    public Text lineText;
    public Dialogue dialogue;

    [Header("Properties")]
    public float textSpeed; // characters per second

    private string currentLine; // [TODO] Use a queue
    private int nextLineIndex;
    private bool advanceScript;

    private Mask inactiveMask;

    #region Singleton pattern
    private static ScreenDialogueBehaviour _current;
    public static ScreenDialogueBehaviour current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType<ScreenDialogueBehaviour>();

                if (_current == null)
                {
                    GameObject container = new GameObject("Singleton Container");
                    _current = container.AddComponent<ScreenDialogueBehaviour>();
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
        return true;
    }
    #endregion

    public void Awake()
    {
        if (EnsureSingleton() == false) return;
        if (input == null) enabled = false;

        inactiveMask = GetComponent<Mask>();
        if (inactiveMask == null) inactiveMask = gameObject.AddComponent<Mask>();
        if (dialogue != null) LoadDialogue(dialogue);
    }

    public void LoadDialogue(Dialogue d)
    {
        dialogue = d;

        if (actorText != null) actorText.text = "";
        if (lineText != null) lineText.text = "";

        nextLineIndex = 0;
        NextLine();
    }

    private void Update()
    {
        if (dialogue == null)
        {
            inactiveMask.enabled = true;
            input.Unlock();
            Time.timeScale = 1;
        }
        else
        {
            inactiveMask.enabled = false;
            input.Lock(); // [TODO] Some speech bubbles shouldn't pause the rest of the game
            Time.timeScale = 0;

            advanceScript = false;
            if (advanceOnInputNames == null || advanceOnInputNames.Count == 0)
            {
                advanceScript = advanceScript || input.GetAnyButtonDown();
            }
            else
            {
                foreach (string inputName in advanceOnInputNames)
                {
                    advanceScript = advanceScript || input.GetButtonDown(inputName);
                }
            }
            if (advanceScript) Advance();
        }

    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Advance();
    }

    public void Advance()
    {
        if (lineText.text == currentLine)
        {
            // If dialogue completed
            if (nextLineIndex >= dialogue.script.Length)
            {
                // [TODO] Put some kind of event here
                dialogue = null;
            }
            else
            {
                NextLine();
            }
        }
        else
        {
            // Display the rest of the current line immediately
            StopCoroutine("DisplayCurrentLine");
            lineText.text = currentLine;
        }
    }

    private void NextLine()
    {
        if (actorText != null) actorText.text = dialogue.script[nextLineIndex].actor.actorName;
        if (lineText != null) lineText.text = "";

        currentLine = dialogue.script[nextLineIndex].line;
        nextLineIndex++;
        StartCoroutine("DisplayCurrentLine");
    }

    private IEnumerator DisplayCurrentLine()
    {
        if (textSpeed <= 0)
        {
            lineText.text = currentLine;
            yield return null;
        }
        else
        {
            for (int i = 0; i < currentLine.Length; i++)
            {
                lineText.text += currentLine[i];
                yield return new WaitForSecondsRealtime(1 / textSpeed);
            }
        }
    }
}
