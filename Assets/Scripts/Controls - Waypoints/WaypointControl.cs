using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CreateAssetMenu(fileName = "waypoints", menuName = "Waypoints")]
public class WaypointScript : ScriptableObject
{
    public Waypoint[] points;
}

[System.Serializable]
public class WaypointEvent
{
    public float t;
    public UnityEvent e;
}

[System.Serializable]
public class Waypoint
{
    public Vector3 position; // Relative to initial position
    public AnimationCurve approachCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float approachCurvature; // Radius of arc path we're following (minus max path deviation), zero for straight line
    public float approachSpeed; // Translates to approach time with path length considered
    public float waitTime; // After reaching position
    public WaypointEvent[] events;

    public Waypoint(Vector3 position)
    {
        this.position = position;
        this.approachCurvature = 0;
        this.approachSpeed = 1f;
        this.waitTime = 0;
        this.events = new WaypointEvent[0];
    }

    public void RunEvents(float t1, float t2)
    {
        foreach (WaypointEvent e in events)
        {
            if (t1 <= e.t && e.t < t2 || !Mathf.Approximately(t1, 1) && Mathf.Approximately(t2, 1) && Mathf.Approximately(e.t, 1))
                e.e.Invoke();
        }
    }

    public float GetTravelTimeFrom(Vector3 startPosition)
    {
        float time = (position - startPosition).magnitude / approachSpeed;
        return time;
    }
}

public class WaypointControl : MonoBehaviour
{
    public Waypoint[] initialPoints = new Waypoint[0];
    public Queue<Waypoint> points = new Queue<Waypoint>();

    private Vector3 anchorPosition;
    private Vector3 lastPosition;
    private Waypoint current;
    private float currentStartTime;
    private float currentCompleteTime;
    private float nextStartTime;

    private void Awake()
    {
        anchorPosition = transform.position;
        if (initialPoints != null)
            foreach (Waypoint point in initialPoints)
                points.Enqueue(point);
    }

    private void Update()
    {
        UpdateTransform();

        if (Time.time >= nextStartTime && points.Count > 0)
        {
            ApplyWaypoint(points.Dequeue());
        }

        if (current != null)
        {
            float t1 = Mathf.InverseLerp(currentStartTime, currentCompleteTime, Time.time - Time.deltaTime);
            float t2 = Mathf.InverseLerp(currentStartTime, currentCompleteTime, Time.time);
            current.RunEvents(t1, t2);
        }
    }

    private void UpdateTransform()
    {
        if (current == null)
            return;
        float t0 = (Time.time - currentStartTime) / (currentCompleteTime - currentStartTime);
        float t1 = current.approachCurve.Evaluate(Mathf.Min(t0, 1));
        transform.position = Vector3.Lerp(lastPosition, anchorPosition + current.position, t1);
    }

    private void ApplyWaypoint(Waypoint w)
    {
        lastPosition = transform.position;
        current = w;
        currentStartTime = Time.time;
        currentCompleteTime = Time.time + w.GetTravelTimeFrom(transform.position - anchorPosition);
        nextStartTime = currentCompleteTime + w.waitTime;
    }
}
