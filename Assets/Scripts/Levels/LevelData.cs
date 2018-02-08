using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Levels
{
    [System.Serializable]
    public class LevelData
    {
        public Transform prefab;
        public string name;
        public string parentName;
        public Vector3 position;

        public List<ScriptableObject> data = new List<ScriptableObject>();

        public LevelData(string name, Transform prefab)
        {
            this.name = name;
            this.parentName = "";
            this.prefab = prefab;
            this.position = Vector3.zero;
            Validate();
        }

        public LevelData(string name, string parentName, Transform prefab, Vector3 position)
        {
            this.name = name;
            this.parentName = parentName;
            this.prefab = prefab;
            this.position = position;
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
}