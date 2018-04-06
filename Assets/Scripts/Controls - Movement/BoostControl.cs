using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidbodyWrapper))]
public class BoostControl : InputBehaviour
{
    [Header("Input")]
    public string buttonName = "Jump";

    [Header("Speed")]
    public bool overrideVelocity = true; // Set velocity along direction instead of adding, usually you want this
    public float magnitude = 5f;
    public Vector3 direction = Vector3.up; // [TODO] auto-normalize this

    protected RigidbodyWrapper mover;

    protected override void Awake()
    {
        base.Awake();
        mover = GetComponent<RigidbodyWrapper>();
    }

    protected virtual void FixedUpdate()
    {
        if (input.GetButtonDown(buttonName))
            Boost(magnitude);
    }

    public virtual void Boost(float magnitude)
    {
        if (overrideVelocity == true)
            magnitude -= Vector3.Dot(mover.Velocity, direction.normalized); // Adjust magnitude to account for current velocity

        mover.AddForce(magnitude * direction.normalized, ForceMode2D.Impulse);
    }
}
