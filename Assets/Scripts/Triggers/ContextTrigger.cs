using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ContextUnityEvent : UnityEvent<Transform> { }

[System.Serializable]
public class ContextTriggerEvents
{
    public ContextUnityEvent onActivate;
    public ContextUnityEvent onDeactivate;
    public ContextUnityEvent onActive;
}

public class ContextTrigger : Trigger {
    public ContextTriggerEvents contextEvents;

    public void DebugLog(Transform other)
    {
        Debug.Log(gameObject.ToString() + ", " + other.ToString());
    }
}
