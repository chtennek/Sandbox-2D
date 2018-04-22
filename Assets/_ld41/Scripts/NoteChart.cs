using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Note Chart", menuName = "LD41/Note Chart")]
public class NoteChart : ScriptableObject
{
    public AudioClip clip;
    public float bpm = 100;
    public float offset;
    public float startMove;
    public float startBeat;
    public float beatIncrement = 1f;
    public float[] beats;
}
