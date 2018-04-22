using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorModifier : ColorModifier
{
    public Image target;

    protected override Color CurrentValue
    {
        get
        {
            if (target == null)
                return Color.clear;

            return target.color;
        }

        set
        {
            if (target == null)
                return;

            target.color = value;
        }
    }

    public void SwitchTarget(Transform other) // [TODO] make this generic?
    {
        Deactivate();

        target = other.GetComponentInParent<Image>();
        BaseValue = CurrentValue; // Overwrite our BaseValue with the new target's value

        Activate();
    }

    protected virtual void Reset()
    {
        target = GetComponent<Image>();
    }

    protected virtual void Awake()
    {
        target = this.GetComponentInTag(targetTag, target);
    }
}
