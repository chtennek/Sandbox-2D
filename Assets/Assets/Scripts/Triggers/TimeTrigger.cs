using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : Trigger
{
    public float delay = 1f;
    public float activeTime = 0f;

    public bool loop = false;
    public float loopWindow = 1f;

    private float t0, previous;

    private void Awake()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        t0 = Time.time;
        previous = -Mathf.Infinity;
    }

    protected void Update()
    {
        float current = (Time.time - t0) - delay; // Trigger activates at active == 0
        float previousInactive = previous - activeTime;
        float currentInactive = current - activeTime;
        if (loop == true)
        {
            current %= loopWindow;
            currentInactive %= loopWindow;
        }

        if (0 <= current && (current < previous || previous < 0))
            Set();

        if (0 <= currentInactive && (currentInactive < previousInactive || previousInactive < 0))
            Unset();

        previous = current;
    }
}
