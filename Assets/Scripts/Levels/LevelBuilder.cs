using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    public class LevelBuilder : MonoBehaviour, IBuilder<Level>
    {
        [SerializeField]
        private Level current;
        public Transform playerPrefab; // [TODO] find a better spot for this

        public Level GetCurrent()
        {
            return current;
        }

        #region Editor functions
        public void Clear()
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Grid>() != null)
                    continue;
                children.Add(child);
            }
            Undo.RecordObjects(children.ToArray(), "Clear LevelBuilder");
            foreach (Transform child in children)
            {
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            foreach (Tilemap tilemap in GetComponentsInChildren<Tilemap>())
            {
                tilemap.ClearAllTiles();
            }
        }

        public void Save()
        {
            List<LevelObjectData> objects = new List<LevelObjectData>();
            foreach (LevelObjectBase objScript in GetComponentsInChildren<LevelObjectBase>())
            {
                LevelObjectData objData = objScript.ToData();
                objects.Add(objData);
            }

            Undo.RecordObject(current, "Save Level Data");
            current.objects = objects.ToArray();
            foreach (Tilemap tilemap in GetComponentsInChildren<Tilemap>())
            {
                current.SaveTilemap(tilemap.gameObject.name, tilemap);
            }
            EditorUtility.SetDirty(current);
            AssetDatabase.SaveAssets();
        }

        public void Load(Level level)
        {
            current = level;
            Reload();
        }

        private void LoadObjects()
        {
            Hashtable objFolders = new Hashtable();
            objFolders[""] = null; // Null transform if no parent specified

            foreach (LevelObjectData objData in current.objects)
            {
                // Find the LevelObject's parent object with caching.
                Transform objFolder;
                if (objFolders.ContainsKey(objData.parentName) == false)
                {
                    objFolder = transform.Find(objData.parentName);
                    if (objFolder == null)
                    {
                        Debug.Log(objData.name + ": Parent (" + objData.parentName + ") not found! Creating...");
                        objFolder = new GameObject(objData.parentName).transform;
                        objFolder.parent = transform;
                        objFolder.localPosition = Vector3.zero;
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
        }

        private void LoadTilemaps() {
            Grid grid = GetComponentInChildren<Grid>();
            string[] tilemapNames = current.GetTilemapNames();
            Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();

            // Create tilemap objects if we need to
            foreach (string tilemapName in tilemapNames) {
                Transform tilemap = grid.transform.Find(tilemapName);
                if (tilemap == null) {
                    Debug.Log(tilemapName + " Tilemap not found! Creating empty tilemap. Check for missing components!");
                    tilemap = new GameObject(tilemapName).transform;
                    tilemap.parent = grid.transform;
                    tilemap.localPosition = Vector3.zero;
                    tilemap.gameObject.AddComponent<Tilemap>();
                    tilemap.gameObject.AddComponent<TilemapRenderer>();
                }
                tilemaps[tilemapName] = tilemap.GetComponent<Tilemap>();
            }

            // Load tilemap data
            foreach (string tilemapName in tilemaps.Keys)
            {
                current.LoadTilemap(tilemapName, tilemaps[tilemapName]);
            }
        }

        public void Reload()
        {
            Clear();
            LoadObjects();
            LoadTilemaps();
        }
        #endregion
    }
}