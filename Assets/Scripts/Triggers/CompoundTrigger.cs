using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompoundTrigger : Trigger {
    public List<Trigger> children = new List<Trigger>();

    private Dictionary<Trigger, UnityAction> activeDelegates;
    private Dictionary<Trigger, UnityAction> inactiveDelegates;
    private HashSet<Trigger> activeChildren;

	private void Awake()
	{
        activeDelegates = new Dictionary<Trigger, UnityAction>();
        inactiveDelegates = new Dictionary<Trigger, UnityAction>();
        activeChildren = new HashSet<Trigger>();

        foreach (Trigger child in children)
        {
            activeDelegates[child] = delegate {
                MarkChildActive(child);
            };
            inactiveDelegates[child] = delegate {
                MarkChildInactive(child);
            };
        }

        foreach (Trigger child in children)
        {
            child.onActivate.AddListener(activeDelegates[child]);
            child.onDeactivate.AddListener(inactiveDelegates[child]);

            if (child.Active == true)
                activeChildren.Add(child);
        }
        Active = activeChildren.Count == children.Count;
    }

	private void OnDestroy()
	{
        foreach (Trigger child in children)
        {
            child.onActivate.RemoveListener(activeDelegates[child]);
            child.onDeactivate.RemoveListener(inactiveDelegates[child]);
        }
	}

    private void MarkChildActive(Trigger child) {
        activeChildren.Add(child);
        Active = activeChildren.Count == children.Count;
    }

    private void MarkChildInactive(Trigger child)
    {
        activeChildren.Remove(child);
        Active = activeChildren.Count == children.Count;
    }
}
