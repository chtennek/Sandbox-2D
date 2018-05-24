﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using DG.Tweening;

public sealed class DialogueBehaviour : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public List<string> advanceOnInputNames;

    [Header("References")]
    public Text actorText;
    public Text lineText;
    public Dialogue dialogue;

    [Tooltip("The UINavigator that should assume control when dialogue needs to be responded to.")]
    public UINavigator navigator;

    [Tooltip("The DialogueResponder whose menu we will pick a response from.")]
    public DialogueResponder dialogueResponder;

    [Header("Properties")]
    public bool playDialogueOnLoad = true;
    public bool setAsMain = false;
    public float textSpeed; // characters per second
    public float autoAdvanceAfter = 1f;
    public bool streamDialogue = true;
    public EaseSettings ease;

    public static DialogueBehaviour main;

    private Queue<Line> lines;
    private Line currentLine;
    private Tweener current;
    private float lastAdvanceTime;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    public void Start()
    {
        lastAdvanceTime = Time.time;
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

        if (streamDialogue && IsDisplayFinished())
            NextLine();
    }

    public void LoadDialogueBranch(string selection)
    {
        if (dialogue == null)
            return;

        foreach (DialogueBranch branch in dialogue.branches)
            if (branch.selection == selection)
                LoadDialogue(branch.dialogue);
    }

    public void LoadDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue;

        lines.Clear();
        foreach (Line line in dialogue.lines)
            lines.Enqueue(line);

        if (dialogueResponder != null)
            dialogueResponder.PopulateMenu(dialogue.branches);

        if (playDialogueOnLoad)
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

        if (autoAdvanceAfter > 0 && Time.time - lastAdvanceTime >= autoAdvanceAfter)
            return true;

        return false;
    }

    public void Advance()
    {
        lastAdvanceTime = Time.time;

        if (IsDisplayFinished()) // Start displaying the next line
            NextLine();
        else if (current != null) // Auto-complete current line
            current.Complete();
    }

    private void NextLine()
    {
        StartCoroutine(Coroutine_NextLine());
    }

    private IEnumerator Coroutine_NextLine()
    {
        yield return null; // Prevent multiple input steps in same frame

        // Display the next line if we have one
        if (lines != null && lines.Count > 0)
        {
            Line line = lines.Dequeue();
            DisplayLine(line);
            yield break;
        }

        // Otherwise display the response menu if we have one
        if (dialogue.skipBranchSelection)
        {
            if (dialogue.branches.Count > 0)
                LoadDialogue(dialogue.branches[0].dialogue);
        }
        else
        {
            if (navigator != null && dialogueResponder != null)
                navigator.MenuOpen(dialogueResponder.menu);
            else
                Warnings.ComponentMissing(this);
        }
    }

    private bool IsDisplayFinished()
    {
        return string.IsNullOrEmpty(currentLine.text) || current == null || current.IsPlaying() == false;
    }

    public void DisplayLine(Line line)
    {
        currentLine = line;

        if (actorText != null)
            actorText.text = line.actor.actorName;

        if (lineText != null)
        {
            string textEndValue = "";
            if (line.preserveDisplay && string.IsNullOrEmpty(lineText.text) == false)
                textEndValue = lineText.text + line.separator;
            else
                lineText.text = "";

            textEndValue += line.text;
            current = lineText.DOText(textEndValue, textSpeed);
            ease.SetEase(current);
        }
    }
}
