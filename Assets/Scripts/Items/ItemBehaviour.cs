using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private InventoryItem item;
    public bool selectable = true;

    public InventoryItem GetItem()
    {
        return item;
    }

    public void SetItem(InventoryItem item)
    {
        this.item = item;
        Refresh();
    }

    private void Awake()
    {
        Refresh();
    }

    private void OnValidate()
    {
        Refresh();
    }

    private void Refresh()
    {
        Image image = transform.GetComponent<Image>();
        if (image != null && item != null)
        {
            image.sprite = item.sprite;
        }
    }

    private void Update()
    {
        if (transform.parent == ItemSelector.current.transform)
        {
            transform.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(1))
            {
                transform.SetParent(ItemSelector.current.transform.parent);
            }
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (selectable && transform.parent != ItemSelector.current.transform)
        {
            // Swap selected item if we are holding one [TODO] make this more robust
            ItemBehaviour item = ItemSelector.current.GetComponentInChildren<ItemBehaviour>();
            ItemSlotBehaviour slot = GetComponentInParent<ItemSlotBehaviour>();
            if (item != null && slot != null)
            {
                slot.DepositItem(item);
            }

            transform.SetParent(ItemSelector.current.transform);
        }
    }
}
