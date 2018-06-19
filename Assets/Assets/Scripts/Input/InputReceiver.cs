using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InputButtonEvent
{
    public string buttonName;

    public UnityEvent onButtonDown;
    public UnityEvent onButton;
    public UnityEvent onButtonUp;
}

[System.Serializable]
public class InputAxisEvent
{
    public string axisName;

    public FloatUnityEvent onAxisDown;
    public FloatUnityEvent onAxis;
}

[System.Serializable]
public class InputAxisPairEvent
{
    public string axisPairName;
    public Grid.CellSwizzle swizzle;

    public Float3UnityEvent onAxisPairDown;
    public Float3UnityEvent onAxisPair;
}

public abstract class InputReceiver : MonoBehaviour
{
    public int playerId = 0;
    public InputButtonEvent[] buttonEvents;
    public InputAxisEvent[] axisEvents;
    public InputAxisPairEvent[] axisPairEvents;
    public readonly float deadZone = .2f;
    public bool restrictToXAxis = false;
    public bool restrictToYAxis = false;

    #region Lock pattern
    [Header("Input Locking")]
    public int priority = 0; // readonly
    public bool lockInput = false;

    private bool isActive = true; // cached lock status based on priority and lockInput
    private static List<InputReceiver> hierarchy = new List<InputReceiver>();

    public void Lock()
    {
        if (lockInput == true)
            return;
        lockInput = true;
        UpdateLockStatuses();
    }

    public void Unlock()
    {
        if (lockInput == false)
            return;
        lockInput = false;
        UpdateLockStatuses();
    }

    private void OnDestroy()
    {
        hierarchy.Remove(this);
        UpdateLockStatuses();
    }

    private void UpdateLockStatuses()
    {
        bool lockSet = false;
        int lockPriority = 0;
        foreach (InputReceiver input in hierarchy)
        {
            if (lockSet == false)
                input.isActive = true;
            else if (input.priority >= lockPriority)
                input.isActive = true;
            else
                input.isActive = false;

            if (lockSet == false && input.lockInput == true)
            {
                lockPriority = input.priority;
                lockSet = true;
            }
        }
    }

    private void Register(InputReceiver input)
    {
        if (hierarchy.Contains(input))
            return;

        for (int i = 0; i <= hierarchy.Count; i++)
            if (i == hierarchy.Count || input.priority > hierarchy[i].priority)
            {
                hierarchy.Insert(i, input);
                break;
            }

        UpdateLockStatuses();
    }
    #endregion

    protected void Start()
    {
        Register(this); // O(n^2)
    }

    private void Update()
    {
        foreach (InputButtonEvent e in buttonEvents)
        {
            if (GetButtonDown(e.buttonName))
                e.onButtonDown.Invoke();

            if (GetButton(e.buttonName))
                e.onButton.Invoke();

            if (GetButtonUp(e.buttonName))
                e.onButtonUp.Invoke();
        }
        foreach (InputAxisEvent e in axisEvents)
        {
            e.onAxisDown.Invoke(GetAxisDown(e.axisName));
            e.onAxis.Invoke(GetAxis(e.axisName));
        }
        foreach (InputAxisPairEvent e in axisPairEvents)
        {
            Vector3 input;

            input = Grid.Swizzle(e.swizzle, GetAxisPairDown(e.axisPairName));
            e.onAxisPairDown.Invoke(input.x, input.y, input.z);

            input = Grid.Swizzle(e.swizzle, GetAxisPair(e.axisPairName));
            e.onAxisPair.Invoke(input.x, input.y, input.z);
        }
    }

    public abstract bool GetButtonDownRaw(string id);
    public abstract bool GetButtonUpRaw(string id);
    public abstract bool GetButtonRaw(string id);
    public abstract bool GetAnyButtonDownRaw();
    public abstract bool GetAnyButtonRaw();
    public abstract float GetAxisRaw(string id);
    public Vector2 GetAxisPairRaw(string axisPairName)
    {
        string horizontal = axisPairName + "Horizontal";
        string vertical = axisPairName + "Vertical";
        float x = GetAxisRaw(horizontal);
        float y = GetAxisRaw(vertical);
        return new Vector2(x, y);
    }

    public abstract float GetAxisDownRaw(string id);
    public Vector2 GetAxisPairDownRaw(string axisPairName)
    {
        string horizontal = axisPairName + "Horizontal";
        string vertical = axisPairName + "Vertical";
        float x = GetAxisDownRaw(horizontal);
        float y = GetAxisDownRaw(vertical);
        return new Vector2(x, y);
    }

    public bool GetButtonDown(string id) { return isActive && GetButtonDownRaw(id); }
    public bool GetButtonUp(string id) { return isActive && GetButtonUpRaw(id); }
    public bool GetButton(string id) { return isActive && GetButtonRaw(id); }
    public bool GetAnyButtonDown() { return isActive && GetAnyButtonDownRaw(); }
    public bool GetAnyButton() { return isActive && GetAnyButtonRaw(); }
    public float GetAxis(string id) { return isActive ? GetAxisRaw(id) : 0; }
    public float GetAxisDown(string id) { return isActive ? GetAxisDownRaw(id) : 0; }

    public Vector2 GetAxisPairDown(string axisPairName) { return GetAxisPairDown(axisPairName, restrictToXAxis, restrictToYAxis); }
    public virtual Vector2 GetAxisPairDown(string axisPairName, bool restrictToXAxis, bool restrictToYAxis)
    {
        if (isActive == false) return Vector2.zero;

        Vector2 input = GetAxisPairDownRaw(axisPairName);

        if (restrictToXAxis && restrictToYAxis)
            input = input.LargestAxis();
        else if (restrictToXAxis)
            input.y = 0;
        else if (restrictToYAxis)
            input.x = 0;

        return input;
    }

    public Vector2 GetAxisPair(string axisPairName) { return GetAxisPair(axisPairName, restrictToXAxis, restrictToYAxis); }
    public Vector2 GetAxisPair(string axisPairName, bool restrictToXAxis, bool restrictToYAxis)
    {
        if (isActive == false) return Vector2.zero;

        Vector2 input = GetAxisPairRaw(axisPairName);
        if (input.magnitude < deadZone)
            return Vector2.zero;

        if (restrictToXAxis && restrictToYAxis)
            input = input.LargestAxis();
        else if (restrictToXAxis)
            input.y = 0;
        else if (restrictToYAxis)
            input.x = 0;

        return input;
    }

    public float GetAxisPairRotation(string axisPairName)
    {
        Vector2 input = GetAxisPair(axisPairName);
        return Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
    }
}
