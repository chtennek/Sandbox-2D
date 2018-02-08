using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Levels
{
    public class LevelBuilder : MonoBehaviour
    {
        public Level currentLevel;

        #region Editor functions
        private void ClearLevel()
        {
            LevelObjectBase[] levelObjects = FindObjectsOfType<LevelObjectBase>();
            Undo.RecordObjects(levelObjects, "Clear Level Objects");
            foreach (LevelObjectBase levelObject in levelObjects)
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
            List<LevelData> objects = new List<LevelData>();

            foreach (LevelObjectBase objScript in FindObjectsOfType<LevelObjectBase>())
            {
                LevelData objData = objScript.ToData();
                objects.Add(objData);
            }

            Undo.RecordObject(currentLevel, "Save Level Data");
            currentLevel.objects = objects.ToArray();
        }

        public void LoadLevel(Level level)
        {
            currentLevel = level;
            ReloadLevel();
        }

        public void ReloadLevel()
        {
            Hashtable objFolders = new Hashtable();
            objFolders[""] = null; // [TODO] eww

            ClearLevel();
            foreach (LevelData objData in currentLevel.objects)
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