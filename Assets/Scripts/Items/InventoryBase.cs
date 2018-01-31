using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBase : MonoBehaviour
{
    public float maxWeight;

    public abstract void ResetView();

    public abstract bool AddItem(InventoryItem item);
    public int AddItem(InventoryItem item, int count)
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

    public abstract bool RemoveItem(InventoryItem item);
    public int RemoveItem(InventoryItem item, int count)
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
