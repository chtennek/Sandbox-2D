using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    private Selectable firstSelected;
    public InGameMenu parentMenu;

    public DOTweener tweenOnOpen;
    public CanvasGroup canvas;

    public Selectable FirstSelected
    {
        get
        {
            if (firstSelected == null)
                firstSelected = GetComponentInChildren<Selectable>();
            return firstSelected;
        }
    }

    public bool Enabled
    {
        get { return canvas != null && canvas.interactable; }
        set
        {
            if (canvas != null)
                canvas.interactable = value;

            if (tweenOnOpen != null)
            {
                if (value == true)
                    tweenOnOpen.PlayForward();
                else
                    tweenOnOpen.PlayBackwards();
            }
        }
    }

    private void Reset()
    {
        tweenOnOpen = GetComponent<DOTweener>();
        canvas = GetComponent<CanvasGroup>();
    }

    protected void Start()
    {
        if (canvas == null)
            Warnings.ComponentMissing<CanvasGroup>(this);

        if (tweenOnOpen != null)
            if (Enabled)
                tweenOnOpen.GotoEnd();
            else
                tweenOnOpen.GotoStart();
    }

    public void Enable()
    {
        Enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
    }

    public void Toggle()
    {
        Enabled = !Enabled;
    }
}
