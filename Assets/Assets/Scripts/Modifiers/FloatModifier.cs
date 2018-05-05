using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

[System.Serializable]
public class FloatModifierParameters
{
    public float multiplier = 1f;
    public float adder;

    public bool setValue;
    public float setter;
}

public abstract class FloatModifier : ModifierBase<float>
{
    public FloatModifierParameters parameters;

    protected override float ModifiedValue()
    {
        float to;
        if (parameters.setValue == true)
            to = parameters.setter;
        else
            to = BaseValue * parameters.multiplier + parameters.adder;

        return to;
    }

    protected override void TweenValueTo(float to)
    {
        if (ease.useCustomCurve)
            DOTween.To(() => CurrentValue, value => CurrentValue = value, to, ease.duration).SetEase(ease.curve);
        else
            DOTween.To(() => CurrentValue, value => CurrentValue = value, to, ease.duration)
                .SetEase(ease.type, ease.overshootOrAmplitude, ease.period);
    }
}
