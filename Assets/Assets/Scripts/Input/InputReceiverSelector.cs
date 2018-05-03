using System.Collections.Generic;
using UnityEngine;

public enum SelectorType
{ // [TODO] use or remove
    Linear,
    LinearWrap,
    Hotkeys,
}

public class InputReceiverSelector : MonoBehaviour
{
    public InputReceiver input;
    public List<InputReceiver> receivers;

    [Header("Input")]
    public string axisName = "Switch";

    [Header("Selector Behaviour")]
    public bool wrap;

    private int _selectedIndex;
    private int SelectedIndex
    {
        get
        {
            return _selectedIndex;
        }
        set
        {
            if (0 <= value && value < receivers.Count)
                Selected = receivers[value];
            else
                Selected = null;
        }
    }
    private InputReceiver Selected
    {
        get
        {
            if (0 <= _selectedIndex && _selectedIndex < receivers.Count)
                return receivers[_selectedIndex];
            return null;
        }
        set
        {
            if (Selected != null)
                Selected.enabled = false;

            _selectedIndex = receivers.IndexOf(value);

            if (Selected != null)
                Selected.enabled = true;
        }
    }

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        foreach (InputReceiver receiver in receivers)
            receiver.enabled = false;

        if (receivers.Count > 0)
            Selected = receivers[0];

        if (input == null)
            Warnings.ComponentMissing<InputReceiver>(this);
    }

    private void Update()
    {
        if (input == null)
            return;

        float movement = input.GetAxisDown(axisName);

        if (movement > 0)
        {
            if (wrap)
                SelectedIndex = (SelectedIndex + 1) % receivers.Count;
            else
                SelectedIndex = Mathf.Min(SelectedIndex + 1, receivers.Count - 1);
        }
        else if (movement < 0)
        {
            if (wrap)
                SelectedIndex = (SelectedIndex - 1 + receivers.Count) % receivers.Count;
            else
                SelectedIndex = Mathf.Max(SelectedIndex - 1, 0);
        }
    }
}
