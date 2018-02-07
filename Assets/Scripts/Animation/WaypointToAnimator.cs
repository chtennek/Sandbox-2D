using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointMovement))]
public class WaypointToAnimator : AnimatorBase
{
    private WaypointMovement wm;

    protected override void Awake()
    {
        base.Awake();
        wm = GetComponent<WaypointMovement>();
    }

    protected override void UpdateAnimation()
    {
        Vector2 movement = (wm.GetCurrentWaypoint() - transform.position).normalized;

        if (parameters.Contains(xParameter))
            anim.SetFloat(xParameter, movement.x);

        if (parameters.Contains(yParameter))
            anim.SetFloat(yParameter, movement.y);
    }
}
