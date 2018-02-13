using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Levels
{
    public class LevelObjectStart : LevelObjectBase
    {
        public LevelStartLocation target;

        public override LevelObjectData ToData()
        {
            LevelObjectData objData = base.ToData();
            if (target != null)
            {
                //target.targetLevel = builder.currentLevel;
                target.position = transform.localPosition;
                objData.data["Target"] = target;
            }
            return objData;
        }

        public override void LoadData(LevelObjectData objData)
        {
            base.LoadData(objData);
            if (objData.data.ContainsKey("Target"))
            {
                target = objData.data["Target"] as LevelStartLocation;
            }
        }
    }
}