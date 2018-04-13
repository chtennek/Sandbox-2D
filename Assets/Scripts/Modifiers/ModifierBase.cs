using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifierBase<T> : MonoBehaviour
{
    [Header("Core")]
    public bool activateOnStart = false;
    public bool applyPermanently = false;
    public float time = Mathf.Infinity;

    [Header("Value")]
    public string targetTag; // Search component's GameObject by tag

    // TypeModifier
    protected T BaseValue;
    protected abstract void ModifyValue(); // Override with setting CurrentValue to modified value

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

    private void Apply()
    {
        ModifyValue();
        if (applyPermanently == true)
            BaseValue = CurrentValue;
    }

    private void Unapply()
    {
        CurrentValue = BaseValue;
    }

    public void Activate()
    {
        if (time == 0)
            return;
        else if (time == Mathf.Infinity)
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
        yield return new WaitForSeconds(time);
        Unapply();
    }
}
