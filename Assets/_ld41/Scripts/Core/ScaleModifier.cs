using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModifier : FloatModifier
{
    protected override float CurrentValue
    {
        get
        {
            return transform.localScale.x;
        }

        set
        {
            transform.localScale = value * Vector3.one;
        }
    }
}
