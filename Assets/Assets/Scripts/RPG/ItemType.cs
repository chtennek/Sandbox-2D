using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox
{
    namespace RPG
    {
        [System.Serializable]
        public struct ItemStack
        {
            public ItemType type;
            public int count;

            public static readonly ItemStack none = new ItemStack(null, 0);

            public ItemStack(ItemType type, int count)
            {
                this.type = type;
                this.count = count;
            }

            public int Add(int amount) {
                if (type == null)
                    return 0;
                
                int previousCount = count;
                count = Mathf.Clamp(count + amount, 0, type.maxPerStack);
                return count - previousCount;
            }
        }

        [CreateAssetMenu(fileName = "ItemType", menuName = "RPG/Item Type", order = 0)]
        public class ItemType : ScriptableObject
        {
            public Sprite image;
            public string displayName = "Sword";
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