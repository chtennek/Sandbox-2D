using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sandbox
{
    namespace RPG
    {
        public class ItemDisplay : MonoBehaviour, IDisplayer<ItemStack>
        {
            [Header("References")]
            [SerializeField]
            private Image image;

            [SerializeField]
            private Text displayName;

            [SerializeField]
            private Text stackSize;

            [SerializeField]
            private Text description;

            public void Display(ItemStack stack)
            {
                ItemType type = stack.type;

                if (image != null)
                    image.sprite = (type == null) ? null : type.image;

                if (displayName != null)
                    displayName.text = (type == null) ? "" : type.displayName;

                if (stackSize != null)
                    stackSize.text = (type == null) ? "" : stack.count.ToString();

                if (description != null)
                    description.text = (type == null) ? "" : type.description;
            }
        }
    }
}
