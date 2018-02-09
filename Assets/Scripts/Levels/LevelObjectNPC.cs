using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Levels;

public class LevelObjectNPC : LevelObjectBase
{
    public override LevelObjectData ToData()
    {
        LevelObjectData objData = base.ToData();
        DialogueOnInteract script = GetComponent<DialogueOnInteract>();
        if (script != null)
        {
            objData.data["Dialogue"] = script.dialogue;
        }
        return objData;
    }

    public override void LoadData(LevelObjectData objData)
    {
        base.LoadData(objData);
        DialogueOnInteract script = GetComponent<DialogueOnInteract>();

        if (script != null && objData.data.ContainsKey("Dialogue"))
        {
            script.dialogue = objData.data["Dialogue"] as Dialogue;
        }
        if (script.dialogue == null)
        {
            Debug.LogWarning(objData.name + ": Failed to load dialogue!");
        }
    }
}
