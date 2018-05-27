using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ContextTriggerEvents
{
    public TransformUnityEvent onActivate;
    public TransformUnityEvent onDeactivate;
    public TransformUnityEvent onActive;
}

public class ContextTrigger : Trigger
{
    public ContextTriggerEvents contextEvents;

    private HashSet<Transform> targets = new HashSet<Transform>();
    public Transform Target
    {
        get
        {
            foreach (Transform other in targets)
                return other;
            return null;
        }
    }

    public bool AddTarget(Transform target)
    {
        if (targets.Add(target) == false)
            return false;

        contextEvents.onActivate.Invoke(target);
        return true;
    }

    public bool RemoveTarget(Transform target)
    {
        if (targets.Remove(target) == false)
            return false;

        contextEvents.onDeactivate.Invoke(target);
        return true;
    }

    protected override void Update()
    {
        Transform[] others = new Transform[targets.Count]; // [TODO] optimize
        targets.CopyTo(others);
        foreach (Transform other in others)
        {
            contextEvents.onActive.Invoke(other);
        }
        base.Update();
    }

    public void DebugLog(Transform other)
    {
        Debug.Log(gameObject.ToString() + ", " + other.ToString());
    }
}
