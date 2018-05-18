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
    public Text actorText;
    public Text lineText;
    public Dialogue dialogue;

    [Header("Properties")]
    public bool setAsMain = false;
    public float textSpeed; // characters per second
    public float autoAdvanceAfter = 1f;
    public bool streamDialogue = true;

    public static DialogueBehaviour main;

    private Queue<Line> lines;
    private IEnumerator current;
    private Line currentLine;
    private int currentLinePosition = -1;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    public void Awake()
    {
        lines = new Queue<Line>();

        if (setAsMain)
            main = this;

        if (input == null || lineText == null)
            enabled = false;

        if (dialogue != null)
            LoadDialogue(dialogue);
    }

    public void AddLine(Line line)
    {
        lines.Enqueue(line);

        if (streamDialogue && (current == null || IsDisplayFinished()))
            NextLine();
    }

    public void LoadDialogue(Dialogue d)
    {
        dialogue = d;
        lines.Clear();
        foreach (Line line in dialogue.lines)
            lines.Enqueue(line);

        NextLine();
    }

    private void Update()
    {
        if (DialogueAdvanceRequested())
            Advance();
    }

    private bool DialogueAdvanceRequested()
    {
        if (advanceOnInputNames == null || advanceOnInputNames.Count == 0)
            if (input.GetAnyButtonDown())
                return true;

        foreach (string inputName in advanceOnInputNames)
            if (input.GetButtonDown(inputName))
                return true;

        return false;
    }

    public void Advance()
    {
        if (IsDisplayFinished())
        {
            // If dialogue is finished displaying, load the next line
            NextLine();
        }
        else if (current != null)
        {
            // Display the rest of the current line immediately
            StopCoroutine(current);
            lineText.text += currentLine.text.Substring(currentLinePosition);
        }
    }

    private void NextLine()
    {
        if (lines == null || lines.Count == 0)
            return;

        Line line = lines.Dequeue();
        DisplayLine(line);
    }

    private bool IsDisplayFinished()
    {
        return string.IsNullOrEmpty(currentLine.text) || currentLinePosition == currentLine.text.Length;
    }

    public void DisplayLine(Line line)
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

        if (line.preserveDisplay)
            lineText.text += line.separator;
        else
            lineText.text = "";

        if (textSpeed <= 0 || textSpeed == Mathf.Infinity)
        {
            lineText.text += line.text;
            currentLinePosition = line.text.Length;
            yield break;
        }

        for (currentLinePosition = 0; currentLinePosition < line.text.Length; currentLinePosition++)
        {
            yield return new WaitForSecondsRealtime(1 / textSpeed);
            lineText.text += line.text[currentLinePosition];
        }

        if (autoAdvanceAfter > 0)
        {
            yield return new WaitForSecondsRealtime(autoAdvanceAfter);
            NextLine();
        }
    }
}
