using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpControl : BoostControl
{
    [Header("Jump")]
    public float[] airJumps = new float[0];

    private int currentJump = 0;

    protected override void FixedUpdate()
    {
        bool isGrounded = mover.IsGrounded();
        if (isGrounded) RefreshJumps();

        if (input.GetButtonDown(buttonName))
        {

            float targetHeight = magnitude;
            if (isGrounded == true)
            {
                Jump(targetHeight);
            }
            if (isGrounded == false && currentJump < airJumps.Length)
            {
                targetHeight = airJumps[currentJump];
                currentJump++;
                Jump(targetHeight);
            }
        }
    }

    public void Jump(float targetHeight)
    {
        float acceleration = Vector3.Dot(direction.normalized, mover.GetTotalField());
        Boost(CalculateRequiredSpeed(acceleration, targetHeight));
    }

    private float CalculateRequiredSpeed(float acceleration, float targetHeight)
    {
        return Mathf.Sqrt(2 * Mathf.Abs(acceleration) * targetHeight);
    }

    public void RefreshJumps()
    {
        currentJump = 0;
    }
}
