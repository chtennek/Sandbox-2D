using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/Base")]
    public class LevelBase : ScriptableObject
    {
        public LevelObjectData[] objects;
    }
}