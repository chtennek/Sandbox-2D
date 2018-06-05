using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private string comment;

    public bool active;

    [SerializeField]
    private TransformUnityEvent onSet;

    [SerializeField]
    private TransformUnityEvent onUnset;

    public void Set() { Set(transform); }
    public void Set(Transform target)
    {
        if (active)
            onSet.Invoke(target);
    }

    public void Unset() { Unset(transform); }
    public void Unset(Transform target)
    {
        if (active)
            onUnset.Invoke(target);
    }

    public void Deallocate(Transform target)
    {
        ObjectPooler.Deallocate(target);
    }

    public void Log()
    {
        Debug.Log(transform);
    }

    public void Log(Transform target)
    {
        Debug.Log(target);
    }
}
