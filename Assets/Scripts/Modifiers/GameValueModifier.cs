using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameValueModifier : FloatModifier
{
    public GameValue target;

    protected override float CurrentValue
    {
        get
        {
            if (target == null)
                return 0;

            return target.Value;
        }

        set
        {
            if (target != null)
                target.Value = value;
        }
    }

    protected virtual void Reset()
    {
        target = GetComponentInParent<GameValue>();
    }

    protected virtual void Awake()
    {
        target = this.GetComponentInTag(targetTag, target);
    }
}