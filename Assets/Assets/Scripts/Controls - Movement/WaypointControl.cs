using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Rewired;

public class WaypointControl : MonoBehaviour
{
    [Header("Input")]
    public bool useMouse = false;
    public int mouseButton = 1;
    public LayerMask layerMask = ~0;
    public List<string> tagMask;
    public float mouseRaycast = 1000;

    [Header("Parameters")]
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

    private void FixedUpdate()
    {
        Transform target;
        if (useMouse == true && Input.GetMouseButtonDown(mouseButton))
        {
            target = GetMouseTarget();
            if (target != null)
            {
                ClearWaypoints();
                AddWaypoint(target);
            }
        }

        while (waypoints.Count > 0)
            if (waypoints.Peek() == null || Vector3.Distance(transform.position, waypoints.Peek().position) <= waypointRadius)
                waypoints.Dequeue();
            else
                break;

        if (waypoints.Count == 0)
            return;

        target = waypoints.Peek();
        if (agent != null && agent.enabled == true)
            agent.SetDestination(target.position);
    }

    public void AddWaypoint(Transform target)
    {
        waypoints.Enqueue(target);
    }

    public void ClearWaypoints()
    {
        waypoints.Clear();
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

        Transform waypoint = ObjectPooler.Instantiate(waypointPrefab);
        if (waypoint != null)
            waypoint.position = hit.point;
        return waypoint;
    }
}
