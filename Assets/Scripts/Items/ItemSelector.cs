using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    private static ItemSelector singleton;
    public static ItemSelector current
    {
        get
        {
            if (singleton == null)
            {
                GameObject obj = new GameObject();
                ItemSelector.singleton = obj.AddComponent<ItemSelector>();
            }
            return ItemSelector.singleton;
        }
        set
        {
            ItemSelector.singleton = value;
        }
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != null && singleton != this)
        {
            Destroy(this);
        }
    }
}
