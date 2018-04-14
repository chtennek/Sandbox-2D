using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorModifier : ColorModifier
{
    public Renderer target;

    protected override Color CurrentValue
    {
        get
        {
            if (target == null)
                return Color.clear;

            return target.material.color;
        }

        set
        {
            if (target != null)
                target.material.color = value;
        }
    }

    public void SwitchTarget(Transform other) // [TODO] make this generic?
    {
        Deactivate();

        target = other.GetComponentInParent<Renderer>();
        BaseValue = CurrentValue; // Overwrite our BaseValue with the new target's value

        Activate();
    }

    protected virtual void Reset()
    {
        target = GetComponentInParent<Renderer>();
    }

    protected virtual void Awake()
    {
        target = this.GetComponentInTag(targetTag, target);
    }
}
