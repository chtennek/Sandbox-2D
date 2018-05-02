using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputReceiver : MonoBehaviour
{
    public int playerId = 0;
    public readonly float deadZone = .2f;

    public bool IsActive
    {
        get
        {
            bool isUnlocked = inputLock == null || inputLock == this;
            return isUnlocked && enabled;
        }
    }

    #region Lock pattern
    protected static InputReceiver inputLock; // [TODO] find a better way to do this

    public void ForceLock()
    {
        inputLock = this;
    }

    public bool Lock()
    {
        if (IsActive)
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

    #endregion

    private void Update()
    {

    }

    public abstract bool GetButtonDownRaw(string id);
    public abstract bool GetButtonUpRaw(string id);
    public abstract bool GetButtonRaw(string id);
    public abstract bool GetAnyButtonDownRaw();
    public abstract bool GetAnyButtonRaw();
    public abstract float GetAxisRaw(string id);

    public bool GetButtonDown(string id) { return IsActive && GetButtonDownRaw(id); }
    public bool GetButtonUp(string id) { return IsActive && GetButtonUpRaw(id); }
    public bool GetButton(string id) { return IsActive && GetButtonRaw(id); }
    public bool GetAnyButtonDown() { return IsActive && GetAnyButtonDownRaw(); }
    public bool GetAnyButton() { return IsActive && GetAnyButtonRaw(); }

    public virtual Vector2 GetAxisPairDown(string id) { return new Vector2(GetAxisDown(id + "Horizontal"), GetAxisDown(id + "Vertical")); }
    public virtual float GetAxisDown(string id) { return GetButtonDown(id) ? GetAxis(id) : 0; }

    public float GetAxis(string id)
    {
        float input = GetAxisRaw(id);
        return (IsActive && Mathf.Abs(input) >= deadZone) ? input : 0;
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
        if (IsActive == false) return Vector2.zero;

        Vector2 output = GetAxisPairRaw(axisPairName);
        if (output.magnitude < deadZone)
        {
            return Vector2.zero;
        }
        return output;
    }

    public float GetAxisPairRotation(string axisPairName)
    {
        Vector2 output = GetAxisPair(axisPairName);
        return Mathf.Atan2(output.y, output.x) * Mathf.Rad2Deg;
    }
}
