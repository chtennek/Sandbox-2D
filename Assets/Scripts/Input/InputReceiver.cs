using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputReceiver : MonoBehaviour
{
    public int playerId = 0;
    public float defaultDeadZone = .2f;

    #region Lock pattern
    protected static InputReceiver inputLock;

    public bool Lock()
    {
        if (IsUnlocked())
        {
            inputLock = this;
            return true;
        }
        return false;
    }

    public void Unlock()
    {
        if (inputLock == this) inputLock = null;
    }

    public bool IsUnlocked()
    {
        return inputLock == null || inputLock == this;
    }
    #endregion

    public abstract bool GetButtonDownRaw(string id);
    public abstract bool GetButtonUpRaw(string id);
    public abstract bool GetButtonRaw(string id);
    public abstract bool GetAnyButtonDownRaw();
    public abstract bool GetAnyButtonRaw();

    public bool GetButtonDown(string id) { return IsUnlocked() && GetButtonDownRaw(id); }
    public bool GetButtonUp(string id) { return IsUnlocked() && GetButtonUpRaw(id); }
    public bool GetButton(string id) { return IsUnlocked() && GetButtonRaw(id); }
    public bool GetAnyButtonDown() { return IsUnlocked() && GetAnyButtonDownRaw(); }
    public bool GetAnyButton() { return IsUnlocked() && GetAnyButtonRaw(); }

    public abstract Vector2 GetAxisPairRaw(string axisPairName);

    public Vector2 GetAxisPair(string axisPairName)
    {
        if (IsUnlocked() == false) return Vector2.zero;

        Vector2 inputValues = GetAxisPairRaw(axisPairName);
        if (inputValues.magnitude < defaultDeadZone)
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
        if (output.x > defaultDeadZone)
        {
            output.x = 1;
        }
        else if (output.x < -defaultDeadZone)
        {
            output.x = -1;
        }
        else
        {
            output.x = 0;
        }
        if (output.y > defaultDeadZone)
        {
            output.y = 1;
        }
        else if (output.y < -defaultDeadZone)
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
