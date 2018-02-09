using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/Base")]
    public class Level : ScriptableObject
    {
        public LevelObjectData[] objects;
        public TilemapDataDictionary tilemaps; // Dictionary<string, LevelTilemapData>

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
}