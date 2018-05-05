using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public enum SelectorType
{ // [TODO] use or remove
    Linear,
    LinearWrap,
    Hotkeys,
}

[System.Serializable]
public struct InputReceiverSelectable
{
    public InputReceiver receiver;
    public CinemachineVirtualCameraBase camera;
}

public class InputReceiverSelector : MonoBehaviour
{
    public InputReceiver input;
    public List<InputReceiverSelectable> selectables;

    [Header("Input")]
    public string axisName = "Switch";

    [Header("Selector Behaviour")]
    public int activeCameraPriority = 11;
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
            if (CurrentReceiver != null)
                CurrentReceiver.enabled = false;
            if (CurrentCamera != null)
                CurrentCamera.Priority = 0;

            _selectedIndex = value;

            if (CurrentReceiver != null)
                CurrentReceiver.enabled = true;
            if (CurrentCamera != null)
                CurrentCamera.Priority = activeCameraPriority;
        }
    }
    private InputReceiver CurrentReceiver
    {
        get
        {
            if (0 <= _selectedIndex && _selectedIndex < selectables.Count)
                return selectables[_selectedIndex].receiver;
            return null;
        }
    }
    private CinemachineVirtualCameraBase CurrentCamera
    {
        get
        {
            if (0 <= _selectedIndex && _selectedIndex < selectables.Count)
                return selectables[_selectedIndex].camera;
            return null;
        }
    }

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        // Disable all inactive selectables
        foreach (InputReceiverSelectable selectable in selectables)
        {
            selectable.receiver.enabled = false;
            selectable.camera.Priority = 0;
        }

        // Initialize first one
        SelectedIndex = 0;

        if (input == null)
            Warnings.ComponentMissing<InputReceiver>(this);
    }

    private void Update()
    {
        if (input == null)
            return;

        float movement = input.GetAxisDown(axisName);

        // Switch selectable
        if (movement > 0)
        {
            if (wrap)
                SelectedIndex = (SelectedIndex + 1) % selectables.Count;
            else
                SelectedIndex = Mathf.Min(SelectedIndex + 1, selectables.Count - 1);
        }
        else if (movement < 0)
        {
            if (wrap)
                SelectedIndex = (SelectedIndex - 1 + selectables.Count) % selectables.Count;
            else
                SelectedIndex = Mathf.Max(SelectedIndex - 1, 0);
        }
    }
}
