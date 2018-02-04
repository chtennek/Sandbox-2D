using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
public class TopdownGridWalk : MonoBehaviour
{
    [Header("Input")]
    private InputReceiver input;
    public string axisPairName = "Move";
    [Space]

    public Vector2 gridCellSize = Vector2.one;
    public Vector2 gridCellOffset = Vector2.zero;
    public LayerMask wallColliderMask;

    private Vector2 destination;

    private WaypointMovement wm;
    private Rigidbody2D rb;

    private void Awake()
    {
        wm = GetComponent<WaypointMovement>();
        input = GetComponent<InputReceiver>();
        rb = GetComponent<Rigidbody2D>();

        destination = rb.position;
    }

    private void FixedUpdate()
    {
        if (wm.waypoints.Count == 0)
        {
            Vector2 movement = input.GetAxisPairSingle(axisPairName).normalized;
            if (movement != Vector2.zero)
            {
                Vector2 nextWaypoint = destination + Vector2.Scale(movement, gridCellSize);
                if (Physics2D.OverlapBox(nextWaypoint, gridCellSize / 2, 0, wallColliderMask) == null)
                {
                    wm.waypoints.Enqueue(nextWaypoint);
                    destination = nextWaypoint;
                }
            }
        }
    }
}
