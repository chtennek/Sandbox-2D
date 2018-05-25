using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class GameMenu : MonoBehaviour
{
    [Header("Animation")]
    public DOTweener tweenOnOpen;
    public bool interruptable;
    private IEnumerator coroutine;

    [Space]
    public bool closeMenuOnSubmit = false;
    public bool closeMenuOnCancel = true;

    [Header("References")]
    public CanvasGroup canvas;
    public MenuCursor cursor;
    public GameMenu parentMenu;

    [SerializeField]
    private Selectable m_FirstSelected;
    public Selectable FirstSelected {
        get {
            if (m_FirstSelected == null)
                m_FirstSelected = GetComponentInChildren<Selectable>();
            return m_FirstSelected;
        }
    }

    public bool Enabled
    {
        get { return canvas.interactable; }
        set
        {
            SetEnabled(value);
        }
    }

    private void Reset()
    {
        m_FirstSelected = GetComponentInChildren<Selectable>();
        cursor = GetComponentInChildren<MenuCursor>();
        tweenOnOpen = GetComponent<DOTweener>();
        canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (cursor != null)
            cursor.Highlighted = FirstSelected;

        if (canvas == null)
            Warnings.ComponentMissing<CanvasGroup>(this);

        if (tweenOnOpen != null)
            if (Enabled)
                tweenOnOpen.GotoEnd();
            else
                tweenOnOpen.GotoStart();
    }

    public IEnumerator SetEnabled(bool value)
    {
        if (coroutine != null)
            if (interruptable == false)
                return null;
            else
                StopCoroutine(coroutine);

        coroutine = Coroutine_SetEnabled(value);
        StartCoroutine(coroutine);
        return coroutine;
    }

    private IEnumerator Coroutine_SetEnabled(bool value)
    {
        if (tweenOnOpen == null)
        {
            canvas.interactable = value;
            yield break;
        }

        Debug.Log(value);
        if (value == true)
        {
            tweenOnOpen.PlayForward();
            yield return tweenOnOpen.WaitForEnd();
        }
        else
        {
            tweenOnOpen.PlayBackwards();
            yield return tweenOnOpen.WaitForStart();
        }

        canvas.interactable = value;
    }
}
