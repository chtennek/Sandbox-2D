using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidescrollerJump : MonoBehaviour
{
    // [TODO] implement multijump
    public float jumpSpeed = 8f;
    public float defaultGravityScale = 2f;
    public float jumpReleaseGravityScale = 12f;
    public float fallGravityScale = 4f;
    public int maxDoubleJumps = 1;

    private List<Collider2D> groundTouched = new List<Collider2D>();
    private int doubleJumpsLeft = 1;

    private InputReceiver input;
    private Rigidbody2D rb;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        rb = GetComponent<Rigidbody2D>();
        doubleJumpsLeft = maxDoubleJumps;
    }

    private void FixedUpdate()
    {
        // Refresh double jumps
        if (groundTouched.Count > 0)
        {
            doubleJumpsLeft = maxDoubleJumps;
        }

        // Jump if we are able to
        if (input.Jump() && (doubleJumpsLeft > 0 || groundTouched.Count > 0))
        {
            float targetSpeed = jumpSpeed - rb.velocity.y;
            rb.AddForce(targetSpeed * Vector2.up, ForceMode2D.Impulse);
            if (groundTouched.Count == 0)
            {
                doubleJumpsLeft--;
            }
        }

        // Tweak jump trajectory
        rb.gravityScale = defaultGravityScale;
        if (input.JumpRelease())
        {
            rb.gravityScale = jumpReleaseGravityScale;
        }
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] points = new ContactPoint2D[10];
        collision.GetContacts(points);
        foreach (ContactPoint2D p in points)
        {
            if (p.normal == Vector2.up && !groundTouched.Contains(collision.collider))
            {
                groundTouched.Add(collision.collider);
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundTouched.Remove(collision.collider);
    }
}
