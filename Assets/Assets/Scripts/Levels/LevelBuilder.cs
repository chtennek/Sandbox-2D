using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    public class LevelBuilder : MonoBehaviour, IBuilder
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
            // Get all child objects to destroy
            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            // Destroy children
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

            // Clear tilemaps
            foreach (Tilemap tilemap in GetComponentsInChildren<Tilemap>())
            {
                tilemap.ClearAllTiles();
            }
        }

        public void SaveAll()
        {
            foreach (LevelBuilder builder in GetComponentsInChildren<LevelBuilder>())
            {
                builder.Save();
            }
        }

        public void Save()
        {
            // Get all non-chunk children
            Transform gridChild = null;
            List<Transform> chunkChildren = new List<Transform>();
            List<Transform> levelChildren = new List<Transform>();
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Grid>() != null)
                    gridChild = child;
                else if (child.GetComponent<LevelBuilder>() != null)
                    chunkChildren.Add(child);
                else
                    levelChildren.Add(child);
            }

            // Find level objects to save
            List<LevelObjectData> objects = new List<LevelObjectData>();
            foreach (Transform child in levelChildren)
            {
                foreach (LevelObjectBase objScript in child.GetComponentsInChildren<LevelObjectBase>())
                {
                    LevelObjectData objData = objScript.ToData();
                    objects.Add(objData);
                }
            }

            List<LevelChunk> chunks = new List<LevelChunk>();
            foreach (Transform child in chunkChildren)
            {
                LevelBuilder builder = child.GetComponent<LevelBuilder>();
                if (builder.current == null)
                {
                    Debug.LogWarning("Chunk has no level specified! Skipping save...");
                    continue;
                }
                chunks.Add(new LevelChunk(builder.GetCurrent(), child.localPosition));
            }

            Undo.RecordObject(current, "Save Level Data");

            current.chunks = chunks.ToArray();
            current.objects = objects.ToArray();

            current.ClearTilemaps();
            if (gridChild != null)
            {
                current.gridPosition = gridChild.localPosition;
                foreach (Tilemap tilemap in gridChild.GetComponentsInChildren<Tilemap>())
                {
                    current.SaveTilemap(tilemap.gameObject.name, tilemap);
                }
            }

            //EditorUtility.SetDirty(current);
            AssetDatabase.SaveAssets();
        }

        public void Load(Level level)
        {
            current = level;
            Reload();
        }

        public void Reload()
        {
            if (Application.isPlaying)
            { // [TODO] don't have too many of these
                StartCoroutine("_Reload");
            }
            else
            {
                StartCoroutine("_Reload");
                return;
                Clear();
                if (current != null)
                {
                    LoadTilemaps();
                    LoadObjects();
                    LoadChunks();
                }
            }
        }

        private IEnumerator _Reload()
        {
            Clear();
            yield return null;
            if (current != null)
            {
                Debug.Log("Loading level: " + current.name);
                LoadTilemaps();
                LoadObjects();
                LoadChunks();
            }
        }

        #endregion

        #region Loading functions
        private void LoadChunks()
        {
            if (current.chunks == null)
                return;

            foreach (LevelChunk chunk in current.chunks)
            {
                Transform target = new GameObject("Level Chunk").transform;
                target.parent = transform;
                target.localPosition = chunk.offset;

                LevelBuilder script = target.gameObject.AddComponent<LevelBuilder>();
                script.Load(chunk.level);
            }
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
                Transform target = InstantiateFromPrefab(objData.prefab);
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

        private void LoadTilemaps()
        {
            Grid grid = GetComponentInChildren<Grid>();
            if (grid == null)
            {
                grid = new GameObject("Grid").AddComponent<Grid>();
                grid.transform.parent = transform;
                grid.transform.localPosition = current.gridPosition;
            }

            string[] tilemapNames = current.GetTilemapNames();
            Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();

            // Create tilemap objects if we need to
            foreach (string tilemapName in tilemapNames)
            {
                Transform tilemap = grid.transform.Find(tilemapName);
                if (tilemap == null)
                {
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
        #endregion

        private Transform InstantiateFromPrefab(Transform prefab)
        {
            if (Application.isPlaying)
            {
                return Instantiate(prefab);
            }
            else
            {
                return PrefabUtility.InstantiatePrefab(prefab) as Transform;
            }
        }
    }
}