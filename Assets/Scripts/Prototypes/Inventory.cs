using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSpace;

    private List<Item> items = new List<Item>();

    public List<Item> GetItems()
    {
        return new List<Item>(items);
    }

    public bool HasRoomFor(int size)
    {
        return maxSpace - items.Count >= size;
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= maxSpace)
        {
            return false;
        }
        items.Add(item);
        return true;
    }

    public Item PopItem()
    {
        if (items.Count == 0)
        {
            return null;
        }
        Item item = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);
        return item;
    }
}
