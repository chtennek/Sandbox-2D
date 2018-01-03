using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SidescrollerControlManager : MonoBehaviour
{
    public float defaultGravityScale = 2f;
    [SerializeField]

    public float playerHeight = 0.22f;
    public float collisionCheckWidth = 0.05f;
    public LayerMask groundMask;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (sprite != null)
        {
            if (rb.velocity.x > 0 && sprite.flipX)
                sprite.flipX = false;
            if (rb.velocity.x < 0 && !sprite.flipX)
                sprite.flipX = true;
        }
        if (anim != null)
        {
            anim.SetFloat("xVelocity", rb.velocity.x);
            anim.SetFloat("yVelocity", rb.velocity.y);
            anim.SetBool("isGrounded", IsGrounded());
            anim.SetBool("isAgainstWall", IsGrounded(Vector2.left) || IsGrounded(Vector2.right));
        }
    }

    public bool IsGrounded() { return IsGrounded(Vector2.down, groundMask); }
    public bool IsGrounded(Vector2 direction) { return IsGrounded(direction, groundMask); }
    public bool IsGrounded(Vector2 direction, LayerMask colliderMask)
    {
        bool isGrounded = Physics2D.Raycast(transform.position, direction, 0.5f * playerHeight * transform.lossyScale.y + collisionCheckWidth, colliderMask);
        return isGrounded;
    }
}
