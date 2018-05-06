using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public abstract class ModifierBase<T> : MonoBehaviour
{
    [Header("Core")]
    public string comment = "";
    public bool activateOnStart = false;
    public bool applyPermanently = false;
    public float timeInEffect = Mathf.Infinity;
    public string targetTag; // Search component's GameObject by tag

    [Space]
    protected EaseSettings ease;

    // TypeModifier
    protected T BaseValue;
    protected abstract T ModifiedValue(); // Override with setting CurrentValue to modified value
    protected abstract void TweenValueTo(T to);

    // ConcreteModifier
    protected abstract T CurrentValue { get; set; } // Override with getter and setter for target property

    private IEnumerator current;

    protected virtual void Start()
    {
        BaseValue = CurrentValue;
        if (activateOnStart == true)
            Activate();
    }

    private void OnDestroy()
    {
        Deactivate();
    }

    public void UpdateAndApply()
    {
        BaseValue = CurrentValue;
        Apply();
    }

    public void Apply()
    {
        T to = ModifiedValue();
        TweenValueTo(to);
        if (applyPermanently == true)
            BaseValue = to;
    }

    public void Unapply()
    {
        TweenValueTo(BaseValue);
    }

    public void Activate() // Apply with timed Unapply for buffs, etc.
    {
        if (timeInEffect == 0)
            return;
        else if (timeInEffect == Mathf.Infinity)
            Apply();
        else
        {
            StopCoroutine(current);
            current = Coroutine_Apply();
            StartCoroutine(current);
        }
    }

    public void Deactivate()
    {
        if (current != null)
            StopCoroutine(current);
        Unapply();
    }

    private IEnumerator Coroutine_Apply()
    {
        Apply();
        yield return new WaitForSeconds(timeInEffect);
        Unapply();
    }
}
