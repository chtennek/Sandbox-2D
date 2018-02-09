using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    public class LevelBuilder : MonoBehaviour
    {
        public Level currentLevel;

        #region Editor functions
        public void ClearLevel()
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
            foreach (Tilemap tilemap in FindObjectsOfType<Tilemap>())
            {
                tilemap.ClearAllTiles();
            }
        }

        public void SaveLevel()
        {
            List<LevelObjectData> objects = new List<LevelObjectData>();
            foreach (LevelObjectBase objScript in FindObjectsOfType<LevelObjectBase>()) // [TODO] only look in children of LevelBuilder
            {
                LevelObjectData objData = objScript.ToData();
                objects.Add(objData);
            }

            Undo.RecordObject(currentLevel, "Save Level Data");
            currentLevel.objects = objects.ToArray();
            foreach (Tilemap tilemap in FindObjectsOfType<Tilemap>()) // [TODO] only look in children of LevelBuilder
            {
                currentLevel.SaveTilemap(tilemap.gameObject.name, tilemap);
            }
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

            foreach (LevelObjectData objData in currentLevel.objects)
            {
                // Find the LevelObject's parent object.
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
                    Debug.LogWarning(objScript.gameObject.name + ": LevelObject script not attached! Failed to properly load.");
                }
                else
                {
                    objScript.LoadData(objData);
                }
            }

            foreach (Tilemap tilemap in FindObjectsOfType<Tilemap>())
            {
                currentLevel.LoadTilemap(tilemap.gameObject.name, tilemap);
            }
        }
        #endregion
    }
}