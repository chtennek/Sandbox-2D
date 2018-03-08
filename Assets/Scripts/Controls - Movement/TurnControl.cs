﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControl : InputBehaviour
{
    public string axisPairName = "Aim";

    [Header("Parameters")]
    public float rotationOffset = 90f; // At what movement direction should we be at 0 rotation?
    public float turnSpeed = Mathf.Infinity; // Degrees per frame

    private void FixedUpdate()
    {
        Vector2 movement = input.GetAxisPair(axisPairName);

        if (movement != Vector2.zero)
        {
            float rotationTarget = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + rotationOffset;
            float rotationDelta = Mathv.ClampAngle180(rotationTarget - transform.eulerAngles.z);
            rotationDelta = (rotationDelta > 0) ? Mathf.Min(rotationDelta, turnSpeed) : Mathf.Max(rotationDelta, -turnSpeed);
            transform.Rotate(rotationDelta * Vector3.forward);
        }
    }
}
