using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : ScriptableObject
{
    public string itemName;
    public int currentStack = 1;
    public int maxPerStack = 1;

    public Sprite image;

    public void OnUse()
    {

    }
}
