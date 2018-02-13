using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Levels
{
    public class LevelObjectExit : LevelObjectBase
    {
        public override LevelObjectData ToData()
        {
            LevelObjectData objData = base.ToData();
            LevelExitBehaviour script = GetComponent<LevelExitBehaviour>();
            if (script != null)
            {
                objData.data["Target"] = script.startLocation;
            }
            return objData;
        }

        public override void LoadData(LevelObjectData objData)
        {
            base.LoadData(objData);
            LevelExitBehaviour script = GetComponent<LevelExitBehaviour>();

            if (script != null && objData.data.ContainsKey("Target"))
            {
                script.startLocation = objData.data["Target"] as LevelStartLocation;
            }
        }
    }
}