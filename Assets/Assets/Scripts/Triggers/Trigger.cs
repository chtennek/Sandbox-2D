using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TriggerEvents
{
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;
    public UnityEvent onActive;
    public UnityEvent onInactive;
}

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private string comment;

    public TriggerEvents events;

    protected bool isActive = false;
    public bool Active
    {
        get
        {
            return isActive;
        }
        set
        {
            if (enabled && isActive == false && value == true)
                events.onActivate.Invoke();
            if (enabled && isActive == true && value == false)
                events.onDeactivate.Invoke();

            isActive = value;
        }
    }

    protected virtual void Update()
    {
        if (Active == true)
            events.onActive.Invoke();
        else
            events.onInactive.Invoke();
    }

    public void DebugLog()
    {
        Debug.Log(gameObject);
    }
}
