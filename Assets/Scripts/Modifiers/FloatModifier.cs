using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void ModifyValue()
    {
        if (parameters.setValue == true)
            CurrentValue = parameters.setter;
        else
            CurrentValue = BaseValue * parameters.multiplier + parameters.adder;
    }
}
