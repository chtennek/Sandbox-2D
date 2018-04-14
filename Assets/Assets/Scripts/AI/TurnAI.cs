using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAI : MovementAI
{
    public float turnAngle = 90f;

    [Header("Wall")]
    public ColliderChecker wallCheck;
    public bool isWallCheckRelative = true;

    [Header("Ground")] // [TODO] move this somewhere else and make it more robust
    public bool turnAtLedges = false;
    public ColliderChecker groundCheck;

    private bool wasAboveGround = true;
    private bool isAtLedge = false;

    private void FixedUpdate()
    {
        wasAboveGround |= groundCheck.Raycast(coll2D);
        if (wasAboveGround && (groundCheck.Raycast(coll2D) == false)) {
            wasAboveGround = false;
            isAtLedge = true;
        }

        if (wallCheck.Cast(coll2D))
            Turn();
        else if (turnAtLedges && isAtLedge) {
            isAtLedge = false;
            Turn();
        }
    }

    public void Turn()
    {
        input.movement = Quaternion.AngleAxis(turnAngle, Vector3.forward) * input.movement;
        if (isWallCheckRelative)
            wallCheck.direction = Quaternion.AngleAxis(turnAngle, Vector3.forward) * wallCheck.direction;
    }
}
