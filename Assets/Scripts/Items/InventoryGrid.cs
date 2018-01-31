using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGrid : InventoryBase
{
    // View objects
    [Header("References")]
    public RectTransform itemSlotPrefab;

    [Header("Properties")]
    public Vector2Int size;
    [SerializeField]
    private InventoryItem[,] items;

    public void Awake()
    {
        if (items == null)
        {
            items = new InventoryItem[size.x, size.y];
        }
        else
        {
            size = new Vector2Int(items.GetLength(0), items.GetLength(1));
        }
        ResetView();
    }

    public override void ResetView()
    {
        if (itemSlotPrefab != null)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            for (int i = 0; i < size.x * size.y; i++)
            {
                Transform child = Instantiate(itemSlotPrefab);
                child.SetParent(transform);
            }
        }
    }

    private Vector2Int FindItemIndex(InventoryItem item)
    {
        for (int i = 0; i < items.GetLength(0); i++)
        {
            for (int j = 0; j < items.GetLength(1); j++)
            {
                if (items[i, j] == item)
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    public override bool AddItem(InventoryItem item)
    {
        Vector2Int index = FindItemIndex(null);
        if (index.x < 0)
        {
            return false;
        }
        items[index.x, index.y] = item;
        return true;
    }

    public override bool RemoveItem(InventoryItem item)
    {
        Vector2Int index = FindItemIndex(item);
        if (index.x < 0)
        {
            return false;
        }
        items[index.x, index.y] = null;
        return true;
    }
}
