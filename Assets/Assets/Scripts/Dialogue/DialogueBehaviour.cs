using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class DialogueBehaviour : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public List<string> advanceOnInputNames;

    [Header("References")]
    public Mask hideWindow;
    public Text actorText;
    public Text lineText;
    public Dialogue dialogue;

    [Header("Properties")]
    public float textSpeed; // characters per second

    private Queue<Line> lines;
    private IEnumerator current;
    private Line currentLine;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
        hideWindow = GetComponent<Mask>();
    }

    public void Awake()
    {
        if (input == null || lineText == null) enabled = false;
        if (dialogue != null) LoadDialogue(dialogue);
    }

    public void LoadDialogue(Dialogue d)
    {
        dialogue = d;
        lines.Clear();
        foreach (Line line in dialogue.lines)
            lines.Enqueue(line);

        if (actorText != null) actorText.text = "";
        if (lineText != null) lineText.text = "";

        NextLine();
    }

    private void Update()
    {
        if (dialogue == null)
        {
            input.Unlock();
            return;
        }

        input.Lock();
        if (ShouldAdvanceDialogue())
            Advance();
    }

    private bool ShouldAdvanceDialogue()
    {
        bool shouldAdvance = false;
        if (advanceOnInputNames == null || advanceOnInputNames.Count == 0)
            shouldAdvance = shouldAdvance || input.GetAnyButtonDown();
        else
            foreach (string inputName in advanceOnInputNames)
                shouldAdvance = shouldAdvance || input.GetButtonDown(inputName);
        return shouldAdvance;
    }

    public void Advance()
    {
        if (current == null)
        {
            // If dialogue is finished displaying, load the next line
            NextLine();
        }
        else
        {
            // Display the rest of the current line immediately
            StopCoroutine(current);
            lineText.text = currentLine.text;
        }
    }

    private void NextLine()
    {
        if (lines == null || lines.Count == 0)
            return;

        Line line = lines.Dequeue();
        DisplayLine(line);
    }

    private void DisplayLine(Line line)
    {
        if (current != null)
            StopCoroutine(current);

        current = Coroutine_DisplayLine(line);
        currentLine = line;
        StartCoroutine(current);
    }

    private IEnumerator Coroutine_DisplayLine(Line line)
    {
        if (actorText != null) actorText.text = line.actor.actorName;
        lineText.text = "";

        if (textSpeed <= 0 || textSpeed == Mathf.Infinity)
        {
            lineText.text = line.text;
            yield break;
        }

        for (int i = 0; i < line.text.Length; i++)
        {
            lineText.text += line.text[i];
            yield return new WaitForSecondsRealtime(1 / textSpeed);
        }

        current = null;
    }
}
