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

    [Space]
    [Tooltip("The UINavigator that should assume control when dialogue needs to be responded to.")]
    public MenuNavigator navigator;

    [Tooltip("The GameMenu to open when we start a dialogue.")]
    public GameMenu menu;

    [Tooltip("The MenuPopular whose menu we will pick a response from.")]
    public MenuPopulator responseMenu;

    [Header("Properties")]
    public bool loadDialogueOnAwake = false;
    public bool playDialogueOnLoad = true;
    public bool setAsMain = false;
    public float textSpeed = 60f; // characters per second
    public float autoAdvanceAfter = 0; // [TODO]
    public bool streamDialogue = true;
    public EaseSettings ease;

    public UnityEvent onAdvanceEmpty;

    public static DialogueBehaviour main;

    private Queue<Line> lines = new Queue<Line>();
    private Line currentLine;
    private Tweener tweener;

    private void Reset()
    {
        menu = GetComponent<GameMenu>();
    }

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
        {
            onAdvanceEmpty.Invoke();
            return;
        }

        // Load the first dialogue branch by default
        if (dialogue.skipBranchSelection)
            LoadDialogue(dialogue.branches[0].dialogue);
        // Otherwise display the response menu if we have one
        else if (navigator != null && responseMenu != null)
            navigator.MenuSwitch(responseMenu.menu);
        // Otherwise we have nothing to do
        else
            onAdvanceEmpty.Invoke();
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

            float duration = line.text.Length * (1 / textSpeed);
            tweener = lineText.DOText(textEndValue, duration);
            ease.SetEase(tweener);
        }
    }
}
