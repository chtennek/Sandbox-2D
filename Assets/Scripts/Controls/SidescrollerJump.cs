﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
[RequireComponent(typeof(SidescrollerControlManager))]
public class SidescrollerJump : MonoBehaviour
{
    public float jumpSpeed = 8f;
    public float jumpReleaseGravityScale = 12f;
    public float fallGravityScale = 4f;

    public int maxDoubleJumps = 1;
    public bool allowWallJump = true;
    public Vector2 wallJumpVelocity = new Vector2(0, 8);
    public bool wallRefreshesDoubleJumps = true;

    private int doubleJumpsLeft = 1;

    private InputReceiver input;
    private Rigidbody2D rb;
    private Animator anim;
    private SidescrollerControlManager manager;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        manager = GetComponent<SidescrollerControlManager>();
        doubleJumpsLeft = maxDoubleJumps;
    }

    private void FixedUpdate()
    {
        // Refresh double jumps
        if (doubleJumpsLeft != maxDoubleJumps)
        {
            if (manager.IsGrounded() || (wallRefreshesDoubleJumps && (manager.IsGrounded(Vector2.left) || manager.IsGrounded(Vector2.right))))
                doubleJumpsLeft = maxDoubleJumps;
        }

        // Jump if we are able to
        if (input.Jump())
        {
            float targetSpeed = jumpSpeed - rb.velocity.y;
            // Grounded jump
            if (manager.IsGrounded())
            {
                rb.AddForce(targetSpeed * Vector2.up, ForceMode2D.Impulse);
            }
            // Wall jump
            else if (allowWallJump && (manager.IsGrounded(Vector2.left) || manager.IsGrounded(Vector2.right)))
            {
                Vector2 targetVelocity;
                if (manager.IsGrounded(Vector2.left))
                {
                    targetVelocity = new Vector2(wallJumpVelocity.x - rb.velocity.x, wallJumpVelocity.y - rb.velocity.y);
                }
                else
                {
                    targetVelocity = new Vector2(-wallJumpVelocity.x - rb.velocity.x, wallJumpVelocity.y - rb.velocity.y);
                }
                rb.AddForce(targetVelocity, ForceMode2D.Impulse);
            }
            // Double jump
            else if (doubleJumpsLeft > 0)
            {
                doubleJumpsLeft--;
                rb.AddForce(targetSpeed * Vector2.up, ForceMode2D.Impulse);
            }
        }

        // Tweak jump trajectory
        if (rb.gravityScale == 0) // [TODO] find a better way to prioritize WallCling
            return;

        rb.gravityScale = manager.defaultGravityScale;
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;
        }
        if (input.JumpRelease())
        {
            rb.gravityScale = jumpReleaseGravityScale;
        }
    }
}
