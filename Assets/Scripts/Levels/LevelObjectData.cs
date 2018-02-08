using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct LevelObjectData
{
    public Transform prefab;
    public string name;
    public string parentName;
    public Vector3 position;

    public Hashtable data;

    public LevelObjectData(Transform prefab)
    {
        this.name = "Level Object";
        this.parentName = "";
        this.prefab = prefab;
        this.position = Vector3.zero;
        this.data = new Hashtable();
        Validate();
    }

    public LevelObjectData(string name, string parentName, Transform prefab, Vector3 position)
    {
        this.name = name;
        this.parentName = parentName;
        this.prefab = prefab;
        this.position = position;
        this.data = new Hashtable();
        Validate();
    }

    private void Validate()
    {
        if (this.prefab == null)
        {
            throw new UnassignedReferenceException(this.name + ": LevelObject must have a prefab!");
        }
    }
}