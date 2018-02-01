using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryGrid : InventoryBase
{
    public Vector2Int size;
    [SerializeField]
    private InventoryItem[] items;

    public void Awake()
    {
        ResetView();
    }

    public override void UpdateEntry(Transform child, InventoryItem item)
    {
        items[child.GetSiblingIndex()] = item;
    }

    public override void ResetView()
    {
        if (items == null)
        {
            items = new InventoryItem[size.x * size.y];
        }
        else
        {
            System.Array.Resize<InventoryItem>(ref items, size.x * size.y);
        }
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
                if (items[i] != null)
                {
                    ItemBehaviour item = Instantiate(itemPrefab).GetComponent<ItemBehaviour>();
                    item.SetItem(items[i]);
                    child.GetComponent<ItemSlotBehaviour>().DepositItem(item);
                }
            }
            GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = size.x;
        }
    }

    private Vector2Int FindItemIndex(InventoryItem item)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (items[i * size.y + j] == item)
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
        items[index.x * size.y + index.y] = item;
        return true;
    }

    public override bool RemoveItem(InventoryItem item)
    {
        Vector2Int index = FindItemIndex(item);
        if (index.x < 0)
        {
            return false;
        }
        items[index.x * size.y + index.y] = null;
        return true;
    }
}
