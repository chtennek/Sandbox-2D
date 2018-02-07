using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointMovement))]
[RequireComponent(typeof(InputReceiver))]
public class TopdownGridWalk : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputReceiver input;
    public string axisPairName = "Move";
    [Space]

    public float bufferWindow = 0.1f; // percentage of grid space length
    public Vector2 gridCellSize = Vector2.one;
    public LayerMask wallColliderMask;

    private Vector3 destination;
    private WaypointMovement wm;
    private Collider2D col;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        wm = GetComponent<WaypointMovement>();
        col = GetComponent<Collider2D>();

        destination = transform.position;
    }

    private void Update()
    {
        Vector2 toDestination = destination - transform.position;
        if (wm.WaypointCount() <= 1 && Mathf.Abs(toDestination.x) <= bufferWindow * gridCellSize.x && Mathf.Abs(toDestination.y) <= bufferWindow * gridCellSize.y)
        {
            Vector2 movement = input.GetAxisPairSingle(axisPairName).normalized;
            if (movement != Vector2.zero)
            {
                Vector3 nextWaypoint = destination + Vector3.Scale(movement, gridCellSize);
                if (Physics2D.OverlapBox(nextWaypoint, col.bounds.size, 0, wallColliderMask) == null)
                {
                    wm.AddWaypoint(nextWaypoint);
                    destination = nextWaypoint;
                }
            }
        }
    }
}
