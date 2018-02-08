using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Levels
{
    public class LevelObjectBase : MonoBehaviour
    {
        public virtual LevelData ToData()
        {
            LevelData objData = new LevelData(transform.name, PrefabUtility.GetPrefabParent(transform) as Transform);
            objData.parentName = transform.parent == null ? "" : transform.parent.name;
            objData.position = transform.position;
            return objData;
        }

        public virtual void LoadData(LevelData objData)
        {
            transform.name = objData.name;
            transform.position = objData.position;
        }
    }
}