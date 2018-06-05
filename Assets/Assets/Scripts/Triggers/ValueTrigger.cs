using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueTrigger : Trigger
{
    [SerializeField]
    private GameValue value;

    [SerializeField]
    private bool sendEventOnAwake;

    public float valueMin = -Mathf.Infinity;
    public float valueMax = Mathf.Infinity;

    private bool lastSetValue;

    private void Reset()
    {
        value = GetComponentInParent<GameValue>();
    }

    private void Awake()
    {
        if (value == null)
            enabled = false;
    }

    private void Start()
    {
        lastSetValue = (valueMin <= value.Value && value.Value <= valueMax);

        if (sendEventOnAwake)
            if (lastSetValue)
                Set(transform);
            else
                Unset(transform);
    }

    protected void Update()
    {
        bool currentSetValue = (valueMin <= value.Value && value.Value <= valueMax);

        if (lastSetValue != currentSetValue)
            if (currentSetValue)
                Set();
            else
                Unset();

        lastSetValue = currentSetValue;
    }
}
