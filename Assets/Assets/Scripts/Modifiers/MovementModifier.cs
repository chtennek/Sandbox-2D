using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModifier : FloatModifier
{
    public MoveControl target;

    protected override float CurrentValue
    {
        get
        {
            if (target == null)
                return 0;

            return target.walkSpeed;
        }

        set
        {
            if (target != null)
                target.walkSpeed = value;
        }
    }

    protected virtual void Reset()
    {
        target = GetComponentInParent<MoveControl>();
    }

    protected virtual void Awake()
    {
        target = this.GetComponentInTag(targetTag, target);
    }
}
