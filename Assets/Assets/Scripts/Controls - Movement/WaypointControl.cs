using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointControl : MonoBehaviour
{
    public float waypointRadius = .1f;

    public NavMeshAgent agent;

    public MoveControl move;

    private Queue<Vector3> waypoints = new Queue<Vector3>();

	private void Reset()
	{
        agent = GetComponent<NavMeshAgent>();
        move = GetComponent<MoveControl>();
	}

	private void FixedUpdate()
	{
        if (waypoints.Count == 0)
            return;

        while (Vector3.Distance(transform.position, waypoints.Peek()) <= waypointRadius)
            waypoints.Dequeue();
        
        Vector3 waypoint = waypoints.Peek();
        if (agent != null)
            agent.destination = waypoint;
	}

	public void AddWaypoint(Vector3 position)
    {
        waypoints.Enqueue(position);
    }

    public void ClearWaypoints()
    {
        waypoints.Clear();
    }
}
