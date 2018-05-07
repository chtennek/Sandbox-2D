using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox
{
    namespace RPG
    {
        [CreateAssetMenu(fileName = "ItemType", menuName = "RPG/Item Type", order = 0)]
        public class ItemType : ScriptableObject
        {
            public Sprite image;
            public string itemName = "Sword";
            [TextArea(1, 5)]
            public string description;

            [Header("Category")]
            public string category = "Default"; // [TODO] ScriptableObjectify
            public bool equippable = false;
            public bool consumable = false;
            public int maxPerStack = 1;

            [Header("Stats")]
            public Stat[] stats;
        }
    }
}