using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level Warp", menuName = "Level/Level Warp")]
    public class LevelWarp : ScriptableObject
    {
        public Level targetLevel;
    }
}