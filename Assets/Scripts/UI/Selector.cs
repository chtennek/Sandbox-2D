using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selector : InputBehaviour
{
    public string horizontalAxis = "UIHorizontal";
    public string verticalAxis = "UIVertical";
    public string submitButton = "UISubmit";
    public string cancelButton = "UICancel";

    //public bool moveOneElementPerInput;
    //public float repeatDelay;

    private Selectable selected;

    protected virtual void Update() {
        if (selected == null)
            return;
        
        Vector2 direction = new Vector2(input.GetAxis(horizontalAxis), input.GetAxis(verticalAxis));
        selected = selected.FindSelectable(direction);

        if (input.GetButtonDown(submitButton)) {
            ISubmitHandler handler = selected as ISubmitHandler;
            handler.OnSubmit(null);
        }
        if (input.GetButtonDown(cancelButton))
        {
            ICancelHandler handler = selected as ICancelHandler;
            handler.OnCancel(null);
        }
    }

    public void Select(Selectable item) {
        selected = item;
        return;
        if (item == null)
            return;
        
        // Deselect
        if (item == selected)
        {
            selected = null;
            return;
        }

        // Swap
        if (selected != null && selected != item)
        {
            Transform t1 = selected.transform;
            Transform t2 = item.transform;
            int i1 = t1.GetSiblingIndex();
            int i2 = t2.GetSiblingIndex();

            Transform parent = t1.parent;
            t1.parent = t2.parent;
            t2.parent = parent;

            t1.SetSiblingIndex(i2);
            t2.SetSiblingIndex(i1);

            EventSystem.current.SetSelectedGameObject(selected.gameObject);
            selected = null;
            return;
        }

        // Select
        selected = item;
    }
}
