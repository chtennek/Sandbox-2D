using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PickUpItem : MonoBehaviour
{
    public Tilemap pickupTileLayer;
    public Inventory inventory;

    private InputReceiver input;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
    }

    public void FixedUpdate()
    {
        if (input.player.GetButtonDown("Fire"))
        {
            Vector3Int position = pickupTileLayer.WorldToCell(transform.position);
            MineableTile t = pickupTileLayer.GetTile<MineableTile>(position);
            if (t != null && t.IsPickup() && inventory.AddItem(t.item))
            {
                pickupTileLayer.SetTile(position, null);
            }
            else if (t == null && inventory.GetItems().Count > 0)
            {
                Item item = inventory.PopItem();
                pickupTileLayer.SetTile(position, item.baseTile);
            }
        }
    }
}
