using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatTweener : FloatModifierModifier
{
    [Header("Tweening")]
    public AnimationCurve curve;
    public float tweenTime;

    private float startTime = 0;

    public void Play()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float t = (tweenTime == 0) ? 1 : Mathf.Clamp01((Time.time - startTime) / tweenTime);
        float value = curve.Evaluate(t);

        parameters.multiplier = value;
        Apply();
        target.Apply();
    }
}
