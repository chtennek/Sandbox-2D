using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostControl : MovementBehaviour
{
    [Header("Input")]
    public string buttonName = "Jump";

    [Header("Speed")]
    public bool overrideParallelV = true; // Set velocity along direction instead of adding, usually you want this
    public float magnitude = 5f;
    public Vector3 direction = Vector3.up; // [TODO] auto-normalize this

    protected virtual void FixedUpdate()
    {
        if (input.GetButtonDown(buttonName))
            Boost(magnitude);
    }

    public virtual void Boost(float magnitude)
    {
        if (overrideParallelV == true)
            magnitude -= Vector3.Dot(velocity, direction.normalized); // Adjust magnitude to account for current velocity

        AddForce(magnitude * direction.normalized, ForceMode2D.Impulse);
    }
}
