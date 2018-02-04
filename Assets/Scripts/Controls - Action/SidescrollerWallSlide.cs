using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
[RequireComponent(typeof(SidescrollerControlManager))]
public class SidescrollerWallSlide : MonoBehaviour
{
    [Header("Input")]
    private InputReceiver input;
    public string axisPairName = "Move";
    [Space]

    public float slideSpeed = 1f; // Set to 0 to cling without moving
    public bool requireInput = false; // [TODO] Set to true to require continuously moving into the wall to cling

    private Rigidbody2D rb;
    private SidescrollerControlManager manager;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        rb = GetComponent<Rigidbody2D>();
        manager = GetComponent<SidescrollerControlManager>();
    }

    private void FixedUpdate()
    {
        Vector2 movement = input.GetAxisPairQuantized(axisPairName);
        if (rb.velocity.y <= -slideSpeed && (manager.IsGrounded(Vector2.left) && (!requireInput || movement.x < 0)
            || manager.IsGrounded(Vector2.right) && (!requireInput || movement.x > 0)))
        {
            float targetSpeed = -slideSpeed - rb.velocity.y;
            rb.AddForce(targetSpeed * Vector2.up, ForceMode2D.Impulse);
            rb.gravityScale = 0;
        }
        else if (rb.gravityScale == 0)
        {
            rb.gravityScale = Mathf.Epsilon;
        }
    }
}
