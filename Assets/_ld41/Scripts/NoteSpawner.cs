using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NoteSpawner : EntitySpawner
{
    public Vector3[] offsets;

    public override void Spawn()
    {
        Vector3 offset = offsets.Length == 0 ? Vector3.zero : offsets[Random.Range(0, offsets.Length - 1)]; // [TODO] pick a better rand function?
        Spawn(velocity, offset);
    }
}
