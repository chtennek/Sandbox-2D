using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using DG.Tweening;

public class MenuCursor : MonoBehaviour
{
    public float lerpValue = .5f;

    [SerializeField]
    private Selectable m_Highlighted;
    public Selectable Highlighted
    {
        get { return m_Highlighted; }
        set
        {
            BaseEventData data = new BaseEventData(null);
            if (Highlighted != null)
                Highlighted.OnDeselect(data); // [TODO] doesn't work if another UINavigator exists

            m_Highlighted = value;
            if (Highlighted == null)
                return;
            else
                Highlighted.OnSelect(data);

            transform.SetParent(value.transform);
        }
    }

    private void Update()
    {
        if (m_Highlighted != null)
            transform.position = Vector3.Lerp(transform.position, m_Highlighted.transform.position, lerpValue);
    }

    public void Move(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;

        if (Highlighted == null)
            return;
        
        Selectable target = Highlighted.FindSelectable(direction);
        if (target != null)
            Highlighted = target;
    }
}
