using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompoundTrigger : Trigger
{
    public bool requireAllActive = true;
    public bool requireAllInactive = false; // requireAllActive = false means requireAllInactive = true
    public List<Trigger> triggers = new List<Trigger>();

    private Dictionary<Trigger, UnityAction> activeDelegates;
    private Dictionary<Trigger, UnityAction> inactiveDelegates;
    private HashSet<Trigger> activeChildren;

    private void Awake()
    {
        activeDelegates = new Dictionary<Trigger, UnityAction>();
        inactiveDelegates = new Dictionary<Trigger, UnityAction>();
        activeChildren = new HashSet<Trigger>();

        foreach (Trigger trigger in triggers)
        {
            activeDelegates[trigger] = delegate
            {
                MarkChildActive(trigger);
            };
            inactiveDelegates[trigger] = delegate
            {
                MarkChildInactive(trigger);
            };
        }

        foreach (Trigger trigger in triggers)
        {
            //    trigger.events.onActivate.AddListener(activeDelegates[trigger]);
            //    trigger.events.onDeactivate.AddListener(inactiveDelegates[trigger]);

            //    if (trigger.Active == true)
            //        activeChildren.Add(trigger);
        }
        UpdateActiveStatus();
    }

    private void OnDestroy()
    {
        foreach (Trigger trigger in triggers)
        {
            //trigger.events.onActivate.RemoveListener(activeDelegates[trigger]);
            //trigger.events.onDeactivate.RemoveListener(inactiveDelegates[trigger]);
        }
    }

    private void UpdateActiveStatus()
    {
        //if (requireAllActive == true)
        //    IsSet = activeChildren.Count == triggers.Count;
        //else
        //    IsSet = activeChildren.Count > 0;

        //if (requireAllInactive == true)
        //    IsSet = activeChildren.Count > 0;
    }

    private void MarkChildActive(Trigger trigger)
    {
        activeChildren.Add(trigger);
        UpdateActiveStatus();
    }

    private void MarkChildInactive(Trigger trigger)
    {
        activeChildren.Remove(trigger);
        UpdateActiveStatus();
    }
}
