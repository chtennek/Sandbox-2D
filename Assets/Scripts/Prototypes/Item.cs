using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int maxPerStack = 1;

    public float timeToMine;
    public MineableTile baseTile;
    public MineableTile minedProduct; // [TODO] make a subclass or something for this?
}
