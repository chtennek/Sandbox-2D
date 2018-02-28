using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoostControl : MovementBehaviour
{
    [Header("Input")]
    public string buttonName = "Jump";

    [Header("Speed")]
    public bool overrideVelocity = true; // Set velocity along direction instead of adding, usually you want this
    public float magnitude = 5f;
    public Vector3 direction = Vector3.up;

    protected virtual void FixedUpdate()
    {
        if (input.GetButtonDown(buttonName))
        {
            Boost();
        }
    }

    public void Boost() { Boost(magnitude); }
    public virtual void Boost(float magnitude)
    {
        float speed = magnitude;
        if (overrideVelocity == true)
            speed = magnitude - Vector3.Dot(velocity, direction.normalized);

        AddForce(speed * direction.normalized, ForceMode2D.Impulse);
    }
}
