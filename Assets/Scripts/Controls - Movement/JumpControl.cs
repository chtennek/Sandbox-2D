using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityFieldEffector))]
public class JumpControl : BoostControl
{
    [Header("Jump")]
    public float[] airJumps = new float[0];

    private int currentJump = 0;
    private GravityFieldEffector fields;

    protected override void Awake()
    {
        base.Awake();
        fields = GetComponent<GravityFieldEffector>();
    }

    protected override void FixedUpdate()
    {
        bool isGrounded = IsGrounded();
        if (isGrounded) RefreshJumps();

        if (input.GetButtonDown(buttonName))
        {
            float field = Vector3.Dot(direction.normalized, fields.GetTotalField());

            float targetHeight = magnitude;
            if (isGrounded == false && currentJump < airJumps.Length)
            {
                targetHeight = airJumps[currentJump];
                currentJump++;
            }

            float initialSpeed = Mathf.Sqrt(2 * Mathf.Abs(field) * targetHeight);
            Boost(initialSpeed);
        }
    }

    public void RefreshJumps()
    {
        currentJump = 0;
    }
}
