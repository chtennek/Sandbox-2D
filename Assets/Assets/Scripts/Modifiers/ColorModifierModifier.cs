using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorModifierModifier : ColorModifier
{
    public ColorModifier target;

    protected override Color CurrentValue
    {
        get
        {
            if (target == null)
                return Color.clear;

            return target.parameters.setter;
        }

        set
        {
            if (target == null)
                return;

            target.parameters.setter = value;
        }
    }

    protected virtual void Reset()
    {
        target = GetComponent<ColorModifier>();
    }

    protected virtual void Awake()
    {
        target = this.GetComponentInTag(targetTag, target);
    }
}
