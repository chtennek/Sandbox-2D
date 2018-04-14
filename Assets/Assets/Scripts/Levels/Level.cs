using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
    public class Level : ScriptableObject
    {
        public LevelChunk[] chunks;
        public LevelObjectData[] objects;

        public Vector3 gridPosition;
        public TilemapDataDictionary tilemaps; // Dictionary<string, LevelTilemapData>

        public string[] GetTilemapNames() {
            string[] names = new string[tilemaps.Count];
            tilemaps.Keys.CopyTo(names, 0);
            return names;
        }

        public void ClearTilemaps() {
            tilemaps.Clear();
        }

        public void SaveTilemap(string id, Tilemap source)
        {
            tilemaps[id] = new LevelTilemapData(source);
        }

        public void LoadTilemap(string id, Tilemap target)
        {
            if (tilemaps.ContainsKey(id) == false)
            {
                Debug.LogWarning("TilemapData " + id + " not found!");
                return;
            }
            tilemaps[id].Load(target);
        }
    }

    [System.Serializable]
    public class LevelChunk
    {
        public Level level;
        public Vector3 offset;

        public LevelChunk(Level level, Vector3 offset)
        {
            this.level = level;
            this.offset = offset;
        }
    }
}