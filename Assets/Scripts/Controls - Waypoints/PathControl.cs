using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[System.Serializable]
public class PathEvent
{
    public float t;
    public UnityEvent e;
}

[System.Serializable]
public class PathPoint
{
    public Vector3 position; // Relative to initial position
    public AnimationCurve approachCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float approachCurvature; // Radius of arc path we're following (minus max path deviation), zero for straight line
    public float approachSpeed; // Translates to approach time with path length considered
    public float waitTime; // After reaching position
    public PathEvent[] events;

    public PathPoint(Vector3 position)
    {
        this.position = position;
        this.approachCurvature = 0;
        this.approachSpeed = 1f;
        this.waitTime = 0;
        this.events = new PathEvent[0];
    }

    public PathPoint(Vector3 position, float approachSpeed) : this(position)
    {
        this.approachSpeed = approachSpeed;
    }

    public void RunEvents(float t1, float t2)
    {
        foreach (PathEvent e in events)
        {
            if (t1 <= e.t && e.t < t2)
                e.e.Invoke();
        }
    }

    public float GetTravelTimeFrom(Vector3 startPosition)
    {
        float time = (approachSpeed == 0) ? 0 : (position - startPosition).magnitude / approachSpeed;
        return time;
    }
}

public class PathControl : MonoBehaviour
{
    public bool loopInitialPoints = false;
    public PathPoint[] initialPoints = new PathPoint[0];
    private Queue<PathPoint> points = new Queue<PathPoint>();

    private Vector3 anchorPosition;
    private Vector3 lastPosition;
    private PathPoint current;
    private float currentStartTime;
    private float currentCompleteTime;
    private float nextStartTime;

    public int Count { get { return points.Count; } }

    public void AddWaypoint(Vector3 position) { AddWaypoint(position, 1); }
    public void AddWaypoint(Vector3 position, float speed)
    {
        points.Enqueue(new PathPoint(position - anchorPosition, speed));
    }

    private void InitializePath()
    {
        if (initialPoints != null)
            foreach (PathPoint point in initialPoints)
                points.Enqueue(point);
    }

    private void Awake()
    {
        anchorPosition = transform.position;
        InitializePath();
    }

    private void Update()
    {
        UpdateTransform();
        ProcessEvents();

        if (loopInitialPoints == true && points.Count == 0)
            InitializePath();

        if (Time.time >= nextStartTime && points.Count > 0)
        {
            ApplyWaypoint(points.Dequeue());
            ProcessEvents();
        }
    }

    private void ProcessEvents()
    {
        if (current != null)
        {
            float t1 = Mathf.InverseLerp(currentStartTime, currentCompleteTime, Time.time - Time.deltaTime);
            float t2 = Mathf.InverseLerp(currentStartTime, currentCompleteTime, Time.time);
            if (t1 == 1 || currentStartTime == currentCompleteTime)
            {
                t1 = 1 + Time.time - Time.deltaTime - currentCompleteTime;
                t2 = 1 + Time.time - currentCompleteTime;
            }

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

    private void ApplyWaypoint(PathPoint w)
    {
        lastPosition = transform.position;
        current = w;
        currentStartTime = Time.time;
        currentCompleteTime = Time.time + w.GetTravelTimeFrom(transform.position - anchorPosition);
        nextStartTime = currentCompleteTime + w.waitTime;
    }
}
