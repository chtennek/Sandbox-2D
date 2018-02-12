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
        public Transform playerPrefab; // [TODO] find a better spot for this

        #region Editor functions
        public void ClearLevel()
        {
            LevelObjectBase[] levelObjects = GetComponentsInChildren<LevelObjectBase>();
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
            foreach (Tilemap tilemap in GetComponentsInChildren<Tilemap>())
            {
                tilemap.ClearAllTiles();
            }
        }

        public void SaveLevel()
        {
            List<LevelObjectData> objects = new List<LevelObjectData>();
            foreach (LevelObjectBase objScript in GetComponentsInChildren<LevelObjectBase>())
            {
                LevelObjectData objData = objScript.ToData();
                objects.Add(objData);
            }

            Undo.RecordObject(currentLevel, "Save Level Data");
            currentLevel.objects = objects.ToArray();
            foreach (Tilemap tilemap in GetComponentsInChildren<Tilemap>())
            {
                currentLevel.SaveTilemap(tilemap.gameObject.name, tilemap);
            }
            EditorUtility.SetDirty(currentLevel);
            AssetDatabase.SaveAssets();
        }

        public void LoadLevel(Level level)
        {
            currentLevel = level;
            ReloadLevel();
        }

        public void ReloadLevel()
        {
            Hashtable objFolders = new Hashtable();
            objFolders[""] = null; // Null transform if no parent specified

            ClearLevel();

            foreach (LevelObjectData objData in currentLevel.objects)
            {
                // Find the LevelObject's parent object with caching.
                Transform objFolder;
                if (objFolders.ContainsKey(objData.parentName) == false)
                {
                    objFolder = transform.Find(objData.parentName);
                    if (objFolder == null)
                    {
                        Debug.Log(objData.name + ": Parent (" + objData.parentName + ") not found! Orphaning object...");
                        objFolder = transform;
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
                if (objFolder != null)
                {
                    target.parent = objFolder;
                }

                // Load LevelObjectData
                LevelObjectBase objScript = target.GetComponent<LevelObjectBase>();
                if (objScript == null)
                {
                    Debug.LogWarning(target.name + ": LevelObject script not attached! Failed to properly load.");
                }
                else
                {
                    objScript.LoadData(objData);
                }
            }

            foreach (Tilemap tilemap in GetComponentsInChildren<Tilemap>())
            {
                currentLevel.LoadTilemap(tilemap.gameObject.name, tilemap);
            }
        }
        #endregion
    }
}