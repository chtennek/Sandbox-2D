using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : MonoBehaviour
{
    protected float BaseValue;
    protected abstract float CurrentValue { get; set; }

    [SerializeField] private float multiplier;
    [SerializeField] private float adder;
    [SerializeField] private float time;

    public void Activate() {
        CurrentValue = BaseValue * multiplier + adder;
    }

    public void Deactivate() {
        CurrentValue = BaseValue;
    }
}
