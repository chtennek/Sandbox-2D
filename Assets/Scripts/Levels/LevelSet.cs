using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level Set", menuName = "Level/Level Set")]
    public class LevelSet : ScriptableObject
    {
        public LevelChunk[] chunks;
    }

    [System.Serializable]
    public class LevelChunk {
        public Level level;
        public Vector3 offset;

        public LevelChunk(Level level, Vector3 offset)
        {
            this.level = level;
            this.offset = offset;
        }
    }
}