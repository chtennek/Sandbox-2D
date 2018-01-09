using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WaypointMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Queue<Vector2> waypoints = new Queue<Vector2>();

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector2 waypoint = GetNextWaypoint();
        Vector2 direction = (waypoint - rb.position).normalized;

        rb.velocity = moveSpeed * direction;
        UpdateAnimation();
    }

    private Vector2 GetNextWaypoint()
    {
        while (waypoints.Count > 0)
        {
            Vector2 waypoint = waypoints.Peek();
            if (Vector2.Distance(rb.position, waypoint) >= Time.deltaTime * moveSpeed)
            {
                return waypoint;
            }
            rb.position = waypoint;
            waypoints.Dequeue();
        }
        return rb.position;
    }

    private void UpdateAnimation()
    {
        if (anim != null)
        {
            anim.SetFloat("xVelocity", rb.velocity.x);
            anim.SetFloat("yVelocity", rb.velocity.y);
        }
    }
}
