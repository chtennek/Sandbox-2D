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
        public Level[] levels;
    }
}