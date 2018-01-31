using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotBehaviour : MonoBehaviour, IPointerClickHandler
{
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
        RectTransform rect = item.transform as RectTransform;
        item.transform.SetParent(transform);
        if (rect != null)
        {
            rect.anchorMin = .5f * Vector2.one;
            rect.anchorMax = .5f * Vector2.one;
            rect.anchoredPosition = Vector2.zero;
        }
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
