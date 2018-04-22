using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyOfLineRenderer
{
    Width,
    Offset
}

public class LineRendererModifier : FloatModifier
{
    public LineRenderer[] renderers;
    public PropertyOfLineRenderer property;

    protected override float CurrentValue
    {
        get
        {
            if (renderers.Length == 0)
                return 0;

            switch (property)
            {
                case PropertyOfLineRenderer.Width:
                    return renderers[0].widthMultiplier;
                case PropertyOfLineRenderer.Offset:
                    return renderers[0].material.GetTextureOffset("_MainTex").x;
            }
            return 0;
        }

        set
        {
            switch (property)
            {
                case PropertyOfLineRenderer.Width:
                    foreach (LineRenderer line in renderers)
                        line.widthMultiplier = value;
                    break;
                case PropertyOfLineRenderer.Offset:
                    foreach (LineRenderer line in renderers)
                        line.material.SetTextureOffset("_MainTex", new Vector2(value, 0));
                    break;
            }
        }
    }
}
