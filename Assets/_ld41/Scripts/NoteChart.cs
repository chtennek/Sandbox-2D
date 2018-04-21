using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Note Chart", menuName = "LD41/Note Chart")]
public class NoteChart : ScriptableObject
{
    public AudioClip clip;
    public float bpm;
    public float offset;
    public float[] beats;
}
