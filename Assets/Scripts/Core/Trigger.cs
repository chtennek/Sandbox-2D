using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour
{
    public bool checkOnUpdate = true;
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;
    public UnityEvent onActive;

    private bool lastCheckValue = false;

    protected virtual void Update()
    {
        if (checkOnUpdate == true)
        {
            bool currentCheckValue = Check();

            if (currentCheckValue == true)
                onActive.Invoke();

            if (currentCheckValue == lastCheckValue)
                return;

            if (currentCheckValue == true)
                onActive.Invoke();
            else
                onDeactivate.Invoke();

            lastCheckValue = currentCheckValue;
        }
    }

    protected abstract bool Check();
}
