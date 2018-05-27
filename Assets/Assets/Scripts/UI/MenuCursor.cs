using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using DG.Tweening;

public class MenuCursor : MonoBehaviour
{
    public float lerpValue = .5f;

    private RectTransform rect;

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
            {
                transform.SetParent(null);
                return;
            }

            Highlighted.OnSelect(data);
            transform.SetParent(value.transform);

            if (rect != null)
                rect.sizeDelta = Vector2.zero; // [TODO] make this more flexible
        }
    }

    private void Awake()
    {
        rect = transform as RectTransform;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, lerpValue);
        if (m_Highlighted != null)
            transform.position = Vector3.Lerp(transform.position, m_Highlighted.transform.position, lerpValue);
    }
}
