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
            LevelObjectData objData = new LevelObjectData(transform.name, PrefabUtility.GetPrefabParent(transform) as Transform);
            objData.parentName = transform.parent == null ? "" : transform.parent.name;
            objData.position = transform.localPosition;
            return objData;
        }

        public virtual void LoadData(LevelObjectData objData)
        {
            transform.name = objData.name;
            transform.localPosition = objData.position;
        }

        public void OnLoadError()
        {
            Debug.LogWarning(gameObject.name + ": Failed to load " + GetType().Name + "!");
        }
    }
}