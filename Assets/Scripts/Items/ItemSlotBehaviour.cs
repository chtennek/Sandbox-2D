using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotBehaviour : MonoBehaviour, IPointerClickHandler
{
    private InventoryBase parentInventory;

    private void Awake()
    {
        parentInventory = GetComponentInParent<InventoryBase>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ItemBehaviour item = ItemSelector.current.GetComponentInChildren<ItemBehaviour>();
        if (item != null)
        {
            DepositItem(item);
        }
    }

    public void DepositItem(ItemBehaviour item)
    {
        if (item == null)
        {
            return;
        }
        RectTransform rect = item.transform as RectTransform;
        item.transform.SetParent(transform);
        if (rect != null)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        if (parentInventory != null)
            parentInventory.UpdateEntry(transform, item.GetItem());
    }

    public ItemBehaviour WithdrawItem()
    {
        ItemBehaviour item = GetComponentInChildren<ItemBehaviour>();
        if (item != null)
        {
            item.transform.SetParent(null);
        }
        return item;
    }
}
