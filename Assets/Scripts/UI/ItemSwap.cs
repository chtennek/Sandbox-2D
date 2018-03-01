using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSwap : MonoBehaviour, ISubmitHandler
{
    public static ItemSwap selected;

    public void OnSubmit(BaseEventData eventData)
    {
        if (selected == this) {
            selected = null;
            return;
        }

        if (selected != null && transform.parent != null && selected.transform.parent == transform.parent) {
            int i = transform.GetSiblingIndex();
            int j = selected.transform.GetSiblingIndex();
            transform.SetSiblingIndex(j);
            selected.transform.SetSiblingIndex(i);

            EventSystem.current.SetSelectedGameObject(selected.gameObject);
            selected = null;
            return;
        }

        selected = this;
    }
}
