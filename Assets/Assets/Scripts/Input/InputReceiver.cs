using UnityEngine;

public abstract class InputReceiver : MonoBehaviour
{
    public int playerId = 0;
    public readonly float deadZone = .2f;
    public bool restrictToXAxis = false;
    public bool restrictToYAxis = false;

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

    public virtual float GetAxisDown(string id)
    {
        return GetButtonDown(id) ? GetAxis(id) : 0;
    }

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

    public virtual Vector2 GetAxisPairDown(string axisPairName)
    {
        Vector2 input = GetAxisPair(axisPairName);
        if (GetButtonDown(axisPairName + "Horizontal") == false)
            input.x = 0;
        if (GetButtonDown(axisPairName + "Vertical") == false)
            input.y = 0;

        return input;
    }

    public Vector2 GetAxisPair(string axisPairName)
    {
        if (IsActive == false) return Vector2.zero;

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
