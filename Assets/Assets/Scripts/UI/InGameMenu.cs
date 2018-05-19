using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class InGameMenu : MonoBehaviour
{
    public Selectable firstSelected;
    public InGameMenu parentMenu;

    public DOTweener tweenOnOpen;
    public CanvasGroup canvas;

    public bool Enabled
    {
        get { return canvas != null && canvas.interactable; }
        set
        {
            if (canvas != null)
                canvas.interactable = value;

            if (tweenOnOpen != null) {
                if (value == true) {
                    tweenOnOpen.PlayForward();
                }
                else {
                    tweenOnOpen.PlayBackwards();
                }
            }
        }
    }

    private void Reset()
    {
        tweenOnOpen = GetComponent<DOTweener>();
        canvas = GetComponent<CanvasGroup>();
    }

    protected void Awake()
    {
        if (canvas == null)
            Warnings.ComponentMissing<CanvasGroup>(this);

        if (firstSelected == null)
            firstSelected = GetComponentInChildren<Selectable>();

        if (tweenOnOpen != null)
            if (Enabled)
                tweenOnOpen.GotoEnd();
            else
                tweenOnOpen.Goto(0);
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
