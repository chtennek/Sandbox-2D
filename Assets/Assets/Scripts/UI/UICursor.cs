using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class UICursor : MonoBehaviour
{
    public DOTweener tweener;
    public float lerpValue = .5f;

    private RectTransform rect;

    [SerializeField]
    private Transform m_target;
    public Transform Target
    {
        get { return m_target; }
        set
        {
            if (Target != null && value == null)
            {
                tweener.PlayBackwards();
            }
            else if (Target == null && value != null)
            {
                transform.position = value.position;
                tweener.PlayForward();
            }

            m_target = value;

            RectTransform source = value as RectTransform;
            if (rect != null && source != null)
                rect.sizeDelta = source.sizeDelta;
        }
    }

    private void Reset()
    {
        tweener = GetComponent<DOTweener>();
    }

    private void Awake()
    {
        rect = transform as RectTransform;

        if (tweener != null)
            if (Target != null)
                tweener.GotoEnd();
            else
                tweener.Goto(0);
    }

    private void Update()
    {
        if (Target != null)
            transform.position = Vector3.Lerp(transform.position, Target.position, lerpValue);
    }
}
