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
        public LevelData[] objects;

        private Hashtable tilemaps;

        public void SaveTilemap(string id, ITilemap tileData)
        {
        }
    }
}