using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpControl : BoostControl
{
    public float[] additionalJumps = new float[0];

    private int currentJump = 0;

    protected override void FixedUpdate()
    {
        bool isGrounded = IsGrounded();
        if (isGrounded) RefreshJumps();

        if (input.GetButtonDown(buttonName))
        {
            if (isGrounded)
            {
                Boost();
            }
            else if (currentJump < additionalJumps.Length)
            {
                Boost(additionalJumps[currentJump]);
                currentJump++;
            }
        }
    }

    public void RefreshJumps()
    {
        currentJump = 0;
    }
}
