using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    [System.Serializable]
    public class LevelObjectData
    {
        public Transform prefab;
        public string name;
        public string parentName;
        public Vector3 position;

        public ScriptableObjectDictionary data = new ScriptableObjectDictionary();

        public LevelObjectData(string name, Transform prefab)
        {
            this.name = name;
            this.parentName = "";
            this.prefab = prefab;
            this.position = Vector3.zero;
            Validate();
        }

        public LevelObjectData(string name, string parentName, Transform prefab, Vector3 position)
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

    [System.Serializable]
    public class LevelTilemapData
    {
        public Vector3Int origin;
        public Vector3Int size;
        public TileBase[] tiles;

        public LevelTilemapData(Tilemap tilemap)
        {
            Save(tilemap);
        }

        public void Save(Tilemap tilemap)
        {
            this.origin = tilemap.origin;
            this.size = tilemap.size;
            tiles = new TileBase[tilemap.size.x * tilemap.size.y];
            tiles = tilemap.GetTilesBlock(tilemap.cellBounds);

        }

        public void Load(Tilemap tilemap)
        {
            tilemap.ClearAllTiles();
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    TileBase tile = tiles[x + y * size.x];
                    tilemap.SetTile(new Vector3Int(origin.x + x, origin.y + y, 0), tile);
                }
            }
        }
    }
}