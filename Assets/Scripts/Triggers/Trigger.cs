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
}

public abstract class Trigger : MonoBehaviour
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
        if (isActive == true)
            events.onActive.Invoke();
    }

    public void DebugLog() {
        Debug.Log(gameObject);
    }
}
