using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
public class TopdownFreeWalk : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float minWalkableSpeed = 1f;
    public float inputDeadZone = .2f;

    protected InputReceiver input;
    protected Rigidbody2D rb;
    protected Animator anim;

    void Awake()
    {
        input = GetComponent<InputReceiver>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 movement = input.GetCircularMovementVector(inputDeadZone);
        Vector2 targetVelocity = walkSpeed * movement - rb.velocity;
        rb.AddForce(targetVelocity, ForceMode2D.Impulse);

        UpdateAnimation();
    }

    protected void UpdateAnimation()
    {
        if (anim != null)
        {
            anim.SetFloat("xVelocity", rb.velocity.x);
            anim.SetFloat("yVelocity", rb.velocity.y);
        }
    }
}
