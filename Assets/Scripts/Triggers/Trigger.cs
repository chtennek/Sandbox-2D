using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour
{
    public string label;
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;
    public UnityEvent onActive;

    private bool isActive = false;
    public bool Active {
        get {
            return isActive;
        }
        set {
            if (enabled && isActive == false && value == true)
                onActivate.Invoke();
            if (enabled && isActive == true && value == false)
                onDeactivate.Invoke();

            isActive = value;
        }
    }

    protected virtual void Update()
    {
        if (isActive == true)
            onActive.Invoke();
    }
}
