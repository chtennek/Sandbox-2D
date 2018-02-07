using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyToAnimator : AnimatorBase
{
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void UpdateAnimation()
    {
        if (parameters.Contains(xParameter))
            anim.SetFloat(xParameter, rb.velocity.x);

        if (parameters.Contains(yParameter))
            anim.SetFloat(yParameter, rb.velocity.y);
    }
}
