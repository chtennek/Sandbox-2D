using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox
{
    namespace RPG
    {
        [CreateAssetMenu(fileName = "EntityType", menuName = "RPG/Entity Type", order = 0)]
        public class EntityType : ScriptableObject
        {
            public Transform prefab;
            public Sprite image;
            public string entityName = "Skeleton";
            [TextArea(1, 5)]
            public string description;

            [Space]
            public List<Stat> baseStats;
            public List<EffectType> moves;
        }
    }
}