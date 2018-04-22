using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyOfMaterial
{
    Color,
    TintColor,
    EmissiveColor,
}

public class MaterialColorModifier : ColorModifier
{
    public Renderer target;
    public PropertyOfMaterial property;

    protected override Color CurrentValue
    {
        get
        {
            if (target == null)
                return Color.clear;

            switch (property)
            {
                case (PropertyOfMaterial.Color):
                    return target.material.color;
                case (PropertyOfMaterial.TintColor):
                    return target.material.GetColor("_TintColor");
                case (PropertyOfMaterial.EmissiveColor):
                    return target.material.GetColor("_EmissiveColor");
            }
            return Color.clear;
        }

        set
        {
            if (target == null)
                return;

            switch (property)
            {
                case (PropertyOfMaterial.Color):
                    target.material.color = value;
                    break;
                case (PropertyOfMaterial.TintColor):
                    target.material.SetColor("_TintColor", value);
                    break;
                case (PropertyOfMaterial.EmissiveColor):
                    target.material.SetColor("_EmissiveColor", value);
                    break;
            }
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
        target = GetComponent<Renderer>();
    }

    protected virtual void Awake()
    {
        target = this.GetComponentInTag(targetTag, target);
    }
}
