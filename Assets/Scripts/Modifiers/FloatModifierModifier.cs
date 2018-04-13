using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyOfFloatModifier
{
    Multiplier,
    Adder,
    Setter
}

public class FloatModifierModifier : FloatModifier
{
    public FloatModifier target;
    public PropertyOfFloatModifier property;

    protected override float CurrentValue
    {
        get
        {
            if (target == null)
                return 0;

            switch (property)
            {
                case PropertyOfFloatModifier.Multiplier:
                    return target.parameters.multiplier;
                case PropertyOfFloatModifier.Adder:
                    return target.parameters.adder;
                case PropertyOfFloatModifier.Setter:
                    return target.parameters.setter;
                default:
                    return 0;
            }
        }

        set
        {
            if (target == null)
                return;

            switch (property)
            {
                case PropertyOfFloatModifier.Multiplier:
                    target.parameters.multiplier = value;
                    break;
                case PropertyOfFloatModifier.Adder:
                    target.parameters.adder = value;
                    break;
                case PropertyOfFloatModifier.Setter:
                    target.parameters.setter = value;
                    break;
            }
        }
    }

    protected virtual void Reset()
    {
        target = GetComponentInParent<FloatModifier>();
    }

    protected virtual void Awake()
    {
        target = this.GetComponentInTag(targetTag, target);
    }
}
