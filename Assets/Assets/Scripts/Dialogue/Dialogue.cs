using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Line
{
    [TextArea(3, 10)]
    public string text;
    public Actor actor;

    public bool preserveDisplay;
    public string separator;

    public AudioClip audio;
    public UnityEvent onLineComplete;

    public Line(string text, Actor actor = null, bool preserveDisplay = true, string separator = "\n", AudioClip audio = null, UnityEvent onLineComplete = null)
    {
        this.text = text;
        this.actor = actor;
        this.preserveDisplay = preserveDisplay;
        this.separator = separator;
        this.audio = audio;
        this.onLineComplete = onLineComplete;
    }
}

[System.Serializable]
public struct DialogueBranch
{
    public string selection;
    public Dialogue branch;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<Line> lines;

    public List<DialogueBranch> branches;
}
