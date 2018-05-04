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

    public bool GetButtonDown(string id) { return IsActive && GetButtonDownRaw(id); }
    public bool GetButtonUp(string id) { return IsActive && GetButtonUpRaw(id); }
    public bool GetButton(string id) { return IsActive && GetButtonRaw(id); }
    public bool GetAnyButtonDown() { return IsActive && GetAnyButtonDownRaw(); }
    public bool GetAnyButton() { return IsActive && GetAnyButtonRaw(); }
    public float GetAxis(string id) { return IsActive ? GetAxisRaw(id) : 0; }
    public float GetAxisDown(string id) { return IsActive ? GetAxisDownRaw(id) : 0; }

    public virtual Vector2 GetAxisPairDown(string axisPairName)
    {
        if (IsActive == false) return Vector2.zero;

        Vector2 input = GetAxisPairDownRaw(axisPairName);

        if (restrictToXAxis && restrictToYAxis)
            input = input.LargestAxis();
        else if (restrictToXAxis)
            input.y = 0;
        else if (restrictToYAxis)
            input.x = 0;

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
