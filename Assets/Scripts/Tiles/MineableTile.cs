using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class MineableTile : Tile
{
    public Item item;

    public bool IsPickup()
    {
        return item.maxPerStack > 0;
    }

    public bool IsMineable()
    {
        return item.timeToMine > 0;
    }

    public void OnPickup(Inventory inventory)
    {
        if (item.maxPerStack > 0)
        {
            inventory.AddItem(item);
        }
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/Mineable Tile")]
    public static void CreateTileAsset()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Mineable Tile", "Mineable Tile", "asset", "Save Tile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MineableTile>(), path);
    }
#endif
}
