using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
public class TopdownFreeWalk : MonoBehaviour
{
    public float walkSpeed = 5f;

    private InputReceiver input;
    private Rigidbody2D rb;

    void Awake()
    {
        input = GetComponent<InputReceiver>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movement = input.GetCircularMovementVector();
        Vector2 targetVelocity = walkSpeed * movement - rb.velocity;
        rb.AddForce(targetVelocity, ForceMode2D.Impulse);
    }
}
