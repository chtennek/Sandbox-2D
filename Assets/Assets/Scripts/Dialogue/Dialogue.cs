using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Line
{
    public Actor actor;
    [TextArea(3, 10)]
    public string text;
    public bool preserveDisplay;

    public AudioClip audio;
    public UnityEvent onLineComplete;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public Line[] lines;
}
