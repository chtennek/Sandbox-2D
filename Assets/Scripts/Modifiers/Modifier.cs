using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : MonoBehaviour
{
    public bool applyPermanently = false;
    [SerializeField] private float multiplier = 1f;
    [SerializeField] private float adder;
    [SerializeField] private float time = Mathf.Infinity;

    protected float BaseValue;
    protected abstract float CurrentValue { get; set; }

    private IEnumerator current;

    protected void Apply()
    {
        CurrentValue = BaseValue * multiplier + adder;
        if (applyPermanently == true)
            BaseValue = CurrentValue;
    }

    protected void Unapply()
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
