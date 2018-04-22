using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputReceiver : MonoBehaviour
{
    public int playerId = 0;
    public readonly float deadZone = .2f;

    #region Lock pattern
    protected static InputReceiver inputLock; // [TODO] find a better way to do this

    public void ForceLock()
    {
        inputLock = this;
    }

    public bool Lock()
    {
        if (IsUnlocked())
        {
            inputLock = this;
            return true;
        }
        Debug.LogWarning("Input locked by: " + inputLock.name);
        return false;
    }

    public void Unlock()
    {
        if (inputLock == this) inputLock = null;
    }

    public bool IsUnlocked()
    {
        bool isUnlocked = inputLock == null || inputLock == this;
        return isUnlocked;
    }
    #endregion

    public abstract bool GetButtonDownRaw(string id);
    public abstract bool GetButtonUpRaw(string id);
    public abstract bool GetButtonRaw(string id);
    public abstract bool GetAnyButtonDownRaw();
    public abstract bool GetAnyButtonRaw();
    public abstract float GetAxisRaw(string id);
    public virtual bool GetPositiveAxisDownRaw(string id) { return GetButtonDown(id) && GetAxisRaw(id) >= deadZone; }
    public virtual bool GetNegativeAxisDownRaw(string id) { return GetButtonDown(id) && GetAxisRaw(id) <= deadZone; }

    public bool GetButtonDown(string id) { return IsUnlocked() && GetButtonDownRaw(id); }
    public bool GetButtonUp(string id) { return IsUnlocked() && GetButtonUpRaw(id); }
    public bool GetButton(string id) { return IsUnlocked() && GetButtonRaw(id); }
    public bool GetAnyButtonDown() { return IsUnlocked() && GetAnyButtonDownRaw(); }
    public bool GetAnyButton() { return IsUnlocked() && GetAnyButtonRaw(); }
    public virtual bool GetAxisPairDown(string id) { return GetAxisDown(id + "Horizontal") || GetAxisDown(id + "Vertical"); }
    public virtual bool GetAxisDown(string id) { return GetPositiveAxisDown(id) || GetNegativeAxisDown(id); }
    public virtual bool GetPositiveAxisDown(string id) { return IsUnlocked() && GetPositiveAxisDownRaw(id); }
    public virtual bool GetNegativeAxisDown(string id) { return IsUnlocked() && GetNegativeAxisDownRaw(id); }
    public float GetAxis(string id)
    {
        float input = GetAxisRaw(id);
        return IsUnlocked() && Mathf.Abs(input) >= deadZone ? input : 0;
    }

    public Vector2 GetAxisPairRaw(string axisPairName)
    {
        string horizontal = axisPairName + "Horizontal";
        string vertical = axisPairName + "Vertical";
        float x = GetAxisRaw(horizontal);
        float y = GetAxisRaw(vertical);
        return new Vector2(x, y);
    }

    public Vector2 GetAxisPair(string axisPairName)
    {
        if (IsUnlocked() == false) return Vector2.zero;

        Vector2 inputValues = GetAxisPairRaw(axisPairName);
        if (inputValues.magnitude < deadZone)
        {
            return Vector2.zero;
        }
        return inputValues;
    }

    public Vector2 GetAxisPairSingle(string axisPairName)
    {
        Vector2 output = GetAxisPair(axisPairName);
        if (Mathf.Abs(output.x) >= Mathf.Abs(output.y))
        {
            output.y = 0;
        }
        else
        {
            output.x = 0;
        }
        return output;
    }

    public Vector2 GetAxisPairQuantized(string axisPairName)
    {
        Vector2 output = GetAxisPair(axisPairName);
        if (output.x > deadZone)
        {
            output.x = 1;
        }
        else if (output.x < -deadZone)
        {
            output.x = -1;
        }
        else
        {
            output.x = 0;
        }
        if (output.y > deadZone)
        {
            output.y = 1;
        }
        else if (output.y < -deadZone)
        {
            output.y = -1;
        }
        else
        {
            output.y = 0;
        }
        return output;
    }

    public float GetAxisPairRotation(string axisPairName)
    {
        Vector2 output = GetAxisPair(axisPairName);
        return Mathf.Atan2(output.y, output.x) * Mathf.Rad2Deg;
    }
}
