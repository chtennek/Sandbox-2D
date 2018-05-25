using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputReceiver : MonoBehaviour
{
    public int playerId = 0;
    public readonly float deadZone = .2f;
    public bool restrictToXAxis = false;
    public bool restrictToYAxis = false;

    [Space]
    #region Lock pattern
    public int priority = 0; // readonly
    public bool lockInput = false;

    private bool isUnlocked = true; // cached lock status based on priority and lockInput
    private static List<InputReceiver> hierarchy = new List<InputReceiver>();

    private void Register(InputReceiver input)
    {
        if (hierarchy.Contains(input))
            return;

        for (int i = 0; i <= hierarchy.Count; i++)
            if (hierarchy[i].priority < input.priority)
            {
                hierarchy.Insert(i, input);
                break;
            }

        UpdateLockStatuses();
    }

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
                input.isUnlocked = true;
            else if (lockPriority <= input.priority)
                input.isUnlocked = true;
            else
                input.isUnlocked = false;

            if (lockSet == false && input.lockInput == true)
            {
                lockPriority = input.priority;
                lockSet = true;
            }
        }
    }
    #endregion

    private void Awake()
    {
        Register(this); // O(n^2)
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

    public bool GetButtonDown(string id) { return isUnlocked && GetButtonDownRaw(id); }
    public bool GetButtonUp(string id) { return isUnlocked && GetButtonUpRaw(id); }
    public bool GetButton(string id) { return isUnlocked && GetButtonRaw(id); }
    public bool GetAnyButtonDown() { return isUnlocked && GetAnyButtonDownRaw(); }
    public bool GetAnyButton() { return isUnlocked && GetAnyButtonRaw(); }
    public float GetAxis(string id) { return isUnlocked ? GetAxisRaw(id) : 0; }
    public float GetAxisDown(string id) { return isUnlocked ? GetAxisDownRaw(id) : 0; }

    public Vector2 GetAxisPairDown(string axisPairName) { return GetAxisPairDown(axisPairName, restrictToXAxis, restrictToYAxis); }
    public virtual Vector2 GetAxisPairDown(string axisPairName, bool restrictToXAxis, bool restrictToYAxis)
    {
        if (isUnlocked == false) return Vector2.zero;

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
        if (isUnlocked == false) return Vector2.zero;

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
