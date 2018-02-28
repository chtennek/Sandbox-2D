using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class WaypointMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Queue<Vector3> waypoints = new Queue<Vector3>();
    private Tweener currentTween;

    private void Start()
    {
        StartCoroutine("Run");
    }

    public Vector3 GetCurrentWaypoint()
    {
        if (waypoints.Count == 0)
        {
            return transform.position;
        }
        return waypoints.Peek();
    }

    public int WaypointCount()
    {
        return waypoints.Count;
    }

    public void AddWaypoint(Vector3 position)
    {
        waypoints.Enqueue(position);
    }

    public void ClearWaypoints()
    {
        waypoints.Clear();
    }

    private IEnumerator Run()
    {
        while (true)
        {
            if (waypoints.Count > 0)
            {
                float waitTime = ApplyNextWaypoint();
                yield return new WaitForSeconds(waitTime);
                if (waypoints.Count > 0)
                {
                    waypoints.Dequeue();
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private float ApplyNextWaypoint()
    {
        Vector3 waypoint = GetCurrentWaypoint();
        float travelTime = (waypoint - (Vector3)transform.position).magnitude / moveSpeed;

        currentTween = transform.DOMove(waypoint, travelTime).SetEase(Ease.Linear);
        return travelTime;
    }
}
