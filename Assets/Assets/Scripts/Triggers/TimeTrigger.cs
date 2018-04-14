using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : Trigger
{
    public float delay = 1f;
    public float activeTime = 0f;

    public bool loop = false;
    public float loopWindow = 1f;

    private float t0;

    private void Awake()
    {
        t0 = Time.time;
    }

    protected override void Update()
    {
        float relativeTime = Time.time - delay;
        if (loop == true)
            relativeTime %= loopWindow;

        if (activeTime == 0)
        {
            if (0 <= relativeTime && relativeTime < Time.deltaTime)
                events.onActivate.Invoke();
        }
        else
        {
            if (0 <= relativeTime && relativeTime < activeTime)
                Active = true;
            else
                Active = false;

            base.Update();
        }
    }
}
