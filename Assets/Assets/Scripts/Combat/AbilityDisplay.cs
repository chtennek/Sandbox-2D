using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sandbox.RPG;

public class AbilityDisplay : MonoBehaviour, IDisplayer<EffectType>
{
    [Header("References")]
    [SerializeField]
    private Image image;

    [SerializeField]
    private Text displayName;

    [SerializeField]
    private Text description;

    private void Reset()
    {
        image = GetComponent<Image>();
        displayName = GetComponent<Text>();
    }

    public void Display(EffectType ability)
    {
        if (image != null)
            image.sprite = (ability == null) ? null : ability.image;

        if (displayName != null)
            displayName.text = (ability == null) ? "" : ability.displayName;

        if (description != null)
            description.text = (ability == null) ? "" : ability.description;
    }
}
