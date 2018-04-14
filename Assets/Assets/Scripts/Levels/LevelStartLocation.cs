using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level Spawn Point", menuName = "Level/Spawn Point")]
    public class LevelStartLocation : ScriptableObject
    {
        public Level targetLevel;
        public Vector3 position;
    }
}