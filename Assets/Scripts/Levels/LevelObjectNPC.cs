using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Levels;

public class LevelObjectNPC : LevelObjectBase
{
    public override LevelData ToData()
    {
        LevelData objData = base.ToData();
        DialogueOnInteract script = GetComponent<DialogueOnInteract>();
        if (script != null)
        {
            objData.data.Add(script.dialogue);
        }
        return objData;
    }

    public override void LoadData(LevelData objData)
    {
        base.LoadData(objData);
        DialogueOnInteract script = GetComponent<DialogueOnInteract>();

        if (script != null && objData.data.Count > 0)
        {
            script.dialogue = objData.data[0] as Dialogue; // [TODO] find a better serialization method
        }
        else
        {
            Debug.LogWarning(objData.name + ": Failed to load dialogue!");
        }
    }
}
