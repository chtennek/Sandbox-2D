using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Rewired;

public class WaypointControl : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public string[] cancelButtonNames = { "Stop" };
    public bool useMouse = false;
    public int mouseButton = 1;
    public LayerMask layerMask = ~0;
    public List<string> tagMask;
    public float mouseRaycast = 1000;

    [Header("Parameters")]
    [Tooltip("Sets how close we have to get to a waypoint before discarding it from the queue. A value that is too low can cause jitter.")]
    public float waypointRadius = .1f;

    [Header("References")]
    public Transform waypointPrefab;
    public NavMeshAgent agent;
    public MoveControl movement;

    private Queue<Transform> waypoints = new Queue<Transform>();

    private void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<MoveControl>();
    }

    private void Awake()
    {
        if (agent != null)
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
        }
    }

    private void FixedUpdate()
    {
        Transform target;

        // Process input
        if (useMouse == true && Input.GetMouseButtonDown(mouseButton))
        {
            target = GetMouseTarget();
            if (target != null)
            {
                ClearWaypoints();
                AddWaypoint(target);
            }
        }
        if (input != null)
            foreach (string buttonName in cancelButtonNames)
                if (input.GetAxisDown(buttonName) != 0)
                    ClearWaypoints();

        // Discard excess waypoints
        while (waypoints.Count > 0)
            if (waypoints.Peek() == null || Vector3.Distance(transform.position, waypoints.Peek().position) <= waypointRadius)
                ObjectPooler.Deallocate(waypoints.Dequeue());
            else
                break;

        // Process current waypoint
        if (waypoints.Count > 0)
        {
            target = waypoints.Peek();
            if (agent != null)
            {
                agent.SetDestination(target.position);
                agent.stoppingDistance = waypointRadius;

                Vector3[] corners = agent.path.corners;
                if (agent.pathPending == false && corners.Length > 0)
                    movement.ApplyMovement((corners[0] - transform.position).normalized);
            }
        }
    }

    public void AddWaypoint(Transform target)
    {
        if (target == null)
            return;
        
        waypoints.Enqueue(target);
    }

    public void ClearWaypoints()
    {
        while (waypoints.Count > 0) {
            ObjectPooler.Deallocate(waypoints.Dequeue());
        }
    }

    private Transform GetMouseTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, mouseRaycast, layerMask) == false)
            return null;

        if (tagMask.Count > 0 && tagMask.Contains(hit.transform.tag) == false)
            return null;

        if (waypointPrefab == null)
            return hit.transform;

        Transform waypoint = ObjectPooler.Allocate(waypointPrefab);
        if (waypoint != null)
            waypoint.position = hit.point;
        return waypoint;
    }
}
