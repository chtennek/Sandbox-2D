using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
public class SidescrollerWalk : MonoBehaviour
{
    [Header("Input")]
    private InputReceiver input;
    public string axisPairName = "Move";
    [Space]

    public float walkSpeed = 5f;
    public float minWalkableSpeed = 1f;
    public float inputDeadZone = .2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float movement = input.GetAxisPairQuantized(axisPairName).x;
        float targetSpeed = walkSpeed * movement - rb.velocity.x;
        rb.AddForce(targetSpeed * Vector2.right, ForceMode2D.Impulse);
    }
}
