using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void ModifyValue()
    {
        if (parameters.setValue == true)
            CurrentValue = parameters.setter;
        else
            CurrentValue = BaseValue.Multiply(parameters.multiplier);
    }
}
