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
            public Text displayName;
            public Text stackSize;
            public Text description;
            public Image image;

            public void Display(ItemStack stack)
            {
                ItemType type = stack.type;

                if (displayName != null)
                    displayName.text = (type == null) ? "" : type.displayName;

                if (stackSize != null)
                    stackSize.text = stack.count.ToString();

                if (description != null)
                    description.text = (type == null) ? "" : type.description;

                if (image != null)
                    image.sprite = (type == null) ? null : type.image;
            }
        }
    }
}
