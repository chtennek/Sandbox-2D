using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

[System.Serializable]
public class ColorModifierParameters
{
    public float multiplier = 1f;

    public bool setValue = true;
    public Color setter = Color.white;
}

public abstract class ColorModifier : ModifierBase<Color>
{
    public ColorModifierParameters parameters;

    protected override Color ModifiedValue()
    {
        Color to;
        if (parameters.setValue == true)
            to = parameters.setter;
        else
            to = BaseValue.Multiply(parameters.multiplier);

        return to;
    }

    protected override void TweenValueTo(Color to)
    {
        if (ease.useCustomCurve)
            DOTween.To(() => CurrentValue, value => CurrentValue = value, to, ease.duration).SetEase(ease.curve);
        else
            DOTween.To(() => CurrentValue, value => CurrentValue = value, to, ease.duration)
                .SetEase(ease.type, ease.overshootOrAmplitude, ease.period);
    }
}
