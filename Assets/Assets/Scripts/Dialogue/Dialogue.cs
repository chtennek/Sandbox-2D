using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public struct Line
{
    [TextArea(3, 10)]
    public string text;
    public Actor actor;

    public bool preserveDisplay;
    public string separator;

    public AudioClip audio;

    public Line(string text, Actor actor = null, bool preserveDisplay = true, string separator = "\n", AudioClip audio = null)
    {
        this.text = text;
        this.actor = actor;
        this.preserveDisplay = preserveDisplay;
        this.separator = separator;
        this.audio = audio;
    }
}

[System.Serializable]
public struct DialogueBranch
{
    public string selection;
    // [TODO] conditionals

    [FormerlySerializedAs("branch")]
    public Dialogue dialogue;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<Line> lines;

    public bool skipBranchSelection = false;
    public List<DialogueBranch> branches;
}
