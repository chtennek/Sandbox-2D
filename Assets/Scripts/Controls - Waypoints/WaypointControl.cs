using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[System.Serializable]
public class WaypointEvent
{
    public float t;
    public UnityEvent e;
}

[System.Serializable]
public class Waypoint
{
    public Vector3 localPosition;
    public float approachCurvature; // Radius of arc path we're following (minus max path deviation), zero for straight line
    public float approachSpeed;
    public float waitTime; // After reaching position
    public WaypointEvent[] events;

    public Waypoint(Vector3 position)
    {
        this.localPosition = position;
        this.approachCurvature = 0;
        this.approachSpeed = 1f;
        this.waitTime = 0;
        this.events = new WaypointEvent[0];
    }

    public void RunEvents(float t1, float t2)
    {
        foreach (WaypointEvent e in events)
        {
            if (t1 <= e.t && e.t < t2)
                e.e.Invoke();
        }
    }

    public float GetTravelTime(Vector3 start)
    {
        return (localPosition - start).magnitude / approachSpeed;
    }

    public Vector3 GetMovementVector(Vector3 start)
    {
        return (localPosition - start).normalized * approachSpeed;
    }
}

[CreateAssetMenu(fileName = "waypoints", menuName = "Waypoints")]
public class WaypointScript : ScriptableObject
{
    public Waypoint[] points;
}

[RequireComponent(typeof(MovementManager))]
public class WaypointControl : MonoBehaviour
{
    public Waypoint[] initialPoints = new Waypoint[0];
    public Queue<Waypoint> points = new Queue<Waypoint>();

    private Waypoint current;
    private float currentStartTime;
    private float currentCompleteTime;
    private float nextStartTime;

    private MovementManager mover;

    private void Awake()
    {
        mover = GetComponent<MovementManager>();
        if (initialPoints != null)
            foreach (Waypoint point in initialPoints)
                points.Enqueue(point);
    }

    private void Update()
    {
        if (Time.time >= nextStartTime && points.Count > 0)
        {
            ApplyWaypoint(points.Dequeue());
        }
        else
        {
            float t1 = Mathf.InverseLerp(currentStartTime, currentCompleteTime, Time.time - Time.deltaTime);
            float t2 = Mathf.InverseLerp(currentStartTime, currentCompleteTime, Time.time);
            current.RunEvents(t1, t2);

            if (Time.time >= currentCompleteTime)
                mover.Velocity = Vector3.zero;
        }
    }

    private void ApplyWaypoint(Waypoint w)
    {
        current = w;
        currentStartTime = Time.time;
        currentCompleteTime = Time.time + w.GetTravelTime(transform.position);
        nextStartTime = currentCompleteTime + w.waitTime;
        mover.Velocity = w.GetMovementVector(transform.position);
    }

    private void ProcessEvents()
    {

    }
}
