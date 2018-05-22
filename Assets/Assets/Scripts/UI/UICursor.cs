using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class UICursor : MonoBehaviour
{
    public DOTweener tweenOnActive;
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
                tweenOnActive.PlayBackwards();
            }
            else if (Target == null && value != null)
            {
                transform.position = value.position;
                tweenOnActive.PlayForward();
            }

            m_target = value;

            RectTransform source = value as RectTransform;
            if (rect != null && source != null)
                rect.sizeDelta = source.sizeDelta;
        }
    }

    private void Reset()
    {
        tweenOnActive = GetComponent<DOTweener>();
    }

    private void Awake()
    {
        rect = transform as RectTransform;

        if (tweenOnActive != null)
            if (Target != null)
                tweenOnActive.GotoEnd();
            else
                tweenOnActive.GotoStart();
    }

    private void Update()
    {
        if (Target != null)
            transform.position = Vector3.Lerp(transform.position, Target.position, lerpValue);
    }
}
