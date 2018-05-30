using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sandbox.RPG;

public class InventoryBehaviour : MonoBehaviour
{
    public ItemStack[] items;
    public bool alwaysCollapse;

    public InventoryDisplay display;

    private void Awake()
    {
        if (display != null)
            display.PopulateMenu(items);
    }

    public void Refresh()
    {
        if (alwaysCollapse)
            Collapse();

        if (display != null)
            display.PopulateMenu(items);
    }

    public void Collapse()
    {
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = i + 1; j < items.Length; j++)
            {
                if (items[i].type == items[j].type)
                {
                    int delta = items[i].Add(items[j].count);
                    items[j].Add(-delta);
                }
            }
        }

        // Null empty stacks
        // [TODO] couple this logic to the item stack?
        for (int i = 0; i < items.Length; i++)
            if (items[i].count == 0)
                items[i] = ItemStack.none;
    }

    public int Add(ItemType type, int amount = 1)
    {
        int total = amount;

        // Add to all existing stacks first
        foreach (ItemStack stack in items)
        {
            if (amount <= 0)
                break;

            if (stack.type != type)
                continue;

            amount -= stack.Add(amount);
        }

        // Create new stacks as needed
        for (int i = 0; i < items.Length; i++)
        {
            if (amount <= 0)
                break;

            if (items[i].type != null)
                continue;

            items[i] = new ItemStack(type, 0);
            amount -= items[i].Add(amount);
        }

        Refresh();
        return total - amount;
    }

    public int Remove(ItemType type, int amount = 1)
    {
        int total = amount;
        for (int i = items.Length - 1; i >= 0; i--)
        {
            if (amount <= 0)
                break;

            if (items[i].type != type)
                continue;

            amount += items[i].Add(-amount);
            if (items[i].count == 0)
                items[i] = ItemStack.none;
        }

        Refresh();
        return total - amount;
    }
}
