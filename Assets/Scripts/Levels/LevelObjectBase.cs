using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Levels
{
    public class LevelObjectBase : MonoBehaviour
    {
        public virtual LevelObjectData ToData()
        {
            LevelObjectData objData = new LevelObjectData(PrefabUtility.GetPrefabParent(transform) as Transform);
            objData.name = transform.name;
            objData.parentName = transform.parent == null ? "" : transform.parent.name;
            objData.position = transform.position;
            return objData;
        }

        public virtual void LoadData(LevelObjectData objData)
        {
            transform.name = objData.name;
            transform.position = objData.position;
        }
    }
}