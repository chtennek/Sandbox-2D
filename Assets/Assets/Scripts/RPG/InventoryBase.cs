using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sandbox.RPG;

public abstract class InventoryBase : MonoBehaviour
{
    // View objects
    [Header("References")]
    public RectTransform itemSlotPrefab;
    public RectTransform itemPrefab;

    [Header("Properties")]
    public float maxWeight;

    public abstract void ResetView();
    public abstract void UpdateEntry(Transform child, ItemType item);

    public abstract bool AddItem(ItemType item);
    public int AddItem(ItemType item, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (AddItem(item) == false)
            {
                return count - i;
            }
        }
        return 0;
    }

    public abstract bool RemoveItem(ItemType item);
    public int RemoveItem(ItemType item, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (RemoveItem(item) == false)
            {
                return count - i;
            }
        }
        return 0;
    }
}
