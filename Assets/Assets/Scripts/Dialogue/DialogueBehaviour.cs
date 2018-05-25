using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DG.Tweening;

public sealed class DialogueBehaviour : MonoBehaviour
{
    [Header("References")]
    public Text actorText;
    public Text lineText;
    public Dialogue dialogue;

    [Tooltip("The UINavigator that should assume control when dialogue needs to be responded to.")]
    public UINavigator navigator;

    [Tooltip("The MenuPopular whose menu we will pick a response from.")]
    public MenuPopulator responseMenu;

    [Header("Properties")]
    public bool playDialogueOnLoad = true;
    public bool setAsMain = false;
    public float textSpeed; // characters per second
    public float autoAdvanceAfter = 1f; // [TODO]
    public bool streamDialogue = true;
    public EaseSettings ease;

    public UnityEvent onAdvanceEmpty;

    public static DialogueBehaviour main;

    private Queue<Line> lines = new Queue<Line>();
    private Line currentLine;
    private Tweener tweener;

    public void Start()
    {
        if (setAsMain)
            main = this;

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

        // Populate options menu after dialogue is complete
        if (responseMenu != null)
        {
            responseMenu.ClearMenu();
            foreach (DialogueBranch branch in dialogue.branches)
                responseMenu.AddMenuItem(branch.selection);
        }

        if (playDialogueOnLoad)
            NextLine();
    }

    public void Advance()
    {
        if (IsDisplayFinished()) // Start displaying the next line
            NextLine();
        else if (tweener != null) // Auto-complete current line
            tweener.Complete();
    }

    private void NextLine()
    {
        // Display the next line if we have one
        if (lines != null && lines.Count > 0)
        {
            Line line = lines.Dequeue();
            DisplayLine(line);
            return;
        }

        if (dialogue.branches.Count == 0)
            return;

        // Otherwise display the response menu if we have one
        if (dialogue.skipBranchSelection)
        {
            LoadDialogue(dialogue.branches[0].dialogue);
        }
        else
        {
            if (navigator != null && responseMenu != null)
                navigator.MenuSwitch(responseMenu.menu);
            else
                Warnings.ComponentMissing(this);
        }
    }

    private bool IsDisplayFinished()
    {
        return string.IsNullOrEmpty(currentLine.text) || tweener == null || tweener.IsPlaying() == false;
    }

    public void DisplayLine(Line line)
    {
        currentLine = line;

        if (actorText != null)
            actorText.text = line.actor.actorName;

        if (lineText != null)
        {
            // Determine if we should flush contents from previous lines
            string textEndValue = "";
            if (line.preserveDisplay && string.IsNullOrEmpty(lineText.text) == false)
                textEndValue = lineText.text + line.separator;
            else
                lineText.text = "";

            textEndValue += line.text;
            tweener = lineText.DOText(textEndValue, textSpeed);
            ease.SetEase(tweener);
        }
    }
}
