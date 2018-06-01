using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sandbox.RPG;

public class InventoryBehaviour : MonoBehaviour
{
    [SerializeField]
    private InventoryDisplay display;

    [SerializeField]
    private bool alwaysCollapse;

    [SerializeField]
    private List<ItemStack> items;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (alwaysCollapse)
            Collapse();

        if (display != null)
            display.Display(items);
    }

    public void Collapse()
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                if (items[i].type == items[j].type)
                {
                    int delta = items[i].Add(items[j].count);
                    items[j].Add(-delta);
                }
            }
        }

        // Set empty stacks to null type
        // [TODO] couple this logic to the item stack?
        for (int i = 0; i < items.Count; i++)
            if (items[i].count == 0)
                items[i].type = null;
    }

    public void Add(Transform target) {
        ItemBehaviour item = target.GetComponent<ItemBehaviour>();
        if (item == null)
            return;

        Add(item.type);
    }

    public int Add(ItemType type, int amount = 1)
    {
        int remaining = amount;

        // Add to all existing stacks first
        foreach (ItemStack stack in items)
        {
            if (remaining <= 0)
                break;

            if (stack.type != type)
                continue;

            remaining -= stack.Add(remaining);
        }

        // Create new stacks as needed
        for (int i = 0; i < items.Count; i++)
        {
            if (remaining <= 0)
                break;

            if (items[i].type != null)
                continue;

            items[i].type = type;
            remaining -= items[i].Add(remaining);
        }

        Refresh();
        return amount - remaining;
    }

    public int Remove(ItemType type, int amount = 1)
    {
        int remaining = amount;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (remaining <= 0)
                break;

            if (items[i].type != type)
                continue;

            remaining += items[i].Add(-remaining);
            if (items[i].count == 0)
                items[i].type = null;
        }

        Refresh();
        return amount - remaining;
    }
}
