using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour {
    public UnityEvent actions;

    protected virtual void Update () {
        if (ConditionsMet() == true)
            actions.Invoke();
	}

    protected abstract bool ConditionsMet();
}
