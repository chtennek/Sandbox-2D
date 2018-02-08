using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Levels
{
    public class LevelBuilder : MonoBehaviour
    {
        public LevelBase currentLevel;

        #region Editor functions
        private void ClearLevel()
        {
            foreach (LevelObjectBase levelObject in FindObjectsOfType<LevelObjectBase>())
            {
                if (Application.isPlaying)
                {
                    Destroy(levelObject.gameObject);
                }
                else
                {
                    DestroyImmediate(levelObject.gameObject);
                }
            }
        }

        public void SaveLevel()
        {
            List<LevelObjectData> objects = new List<LevelObjectData>();

            foreach (LevelObjectBase objScript in FindObjectsOfType<LevelObjectBase>())
            {
                LevelObjectData objData = objScript.ToData();
                objects.Add(objData);
            }

            currentLevel.objects = objects.ToArray();
        }

        public void LoadLevel(LevelBase level)
        {
            currentLevel = level;
            ReloadLevel();
        }

        public void ReloadLevel()
        {
            Hashtable objFolders = new Hashtable();
            objFolders[""] = null; // [TODO] eww

            ClearLevel();
            foreach (LevelObjectData objData in currentLevel.objects)
            {
                // Find or create the new LevelObject's parent folder.
                Transform objFolder;
                if (objFolders.ContainsKey(objData.parentName) == false)
                {
                    objFolder = GameObject.Find(objData.parentName).transform;
                    if (objFolder == null)
                    {
                        Debug.LogWarning(objData.name + ": Parent (" + objData.parentName + ") not found! Orphaning object...");
                    }
                    objFolders[objData.parentName] = objFolder;
                }
                else
                {
                    objFolder = objFolders[objData.parentName] as Transform;
                }

                // Create the LevelObject
                Transform target = PrefabUtility.InstantiatePrefab(objData.prefab) as Transform;
                //Transform target = Instantiate(objData.prefab, objFolder);
                target.parent = objFolder;
                LevelObjectBase objScript = target.GetComponent<LevelObjectBase>();
                if (objScript == null)
                {
                    Debug.LogWarning(objScript.name + ": LevelObject script not attached! Failed to properly load.");
                }
                else
                {
                    objScript.LoadData(objData);
                }
            }
        }
        #endregion
    }
}