using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InventoryItem : ItemBase
{
    public int maxPerStack = 1; // [TODO] handle item stacks

    public float weight = 0; // [TODO] handle weight
    public Vector2Int gridSize = Vector2Int.one; // [TODO] For grid-like inventory systems?

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Item", priority = 0)]
    private static void CreateAssetFromSprite()
    {
        // Figure out path of current selection
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (System.IO.Path.GetExtension(path) != "")
        {
            path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        if (Selection.objects.Length <= 1 && Selection.activeObject.GetType() != typeof(Sprite))
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<InventoryItem>(), AssetDatabase.GenerateUniqueAssetPath(path + "/New Item.asset"));
        }
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i].GetType() != typeof(Sprite))
                {
                    continue;
                }
                Sprite sprite = (Sprite)Selection.objects[i];
                InventoryItem item = ScriptableObject.CreateInstance<InventoryItem>();
                item.sprite = sprite;
                AssetDatabase.CreateAsset(item, AssetDatabase.GenerateUniqueAssetPath(path + "/" + sprite.name + ".asset"));
                EditorUtility.DisplayProgressBar("Creating Assets from Selection", sprite.name, (float)i / Selection.objects.Length);
            }
            EditorUtility.ClearProgressBar();
        }
    }
#endif

}

[System.Serializable]
public struct ItemStack // [TODO]
{
    InventoryItem item;
    int stackSize;
}