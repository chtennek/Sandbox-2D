using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public Quaternion rotation = Quaternion.identity; // Absolute rotation
    public bool affectRotation = false;

    public AnimationCurve approachCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float approachCurvature = 0; // Radius of arc path we're following (minus max path deviation), zero for straight line
    public float approachSpeed = 1f; // Translates to approach time with path length considered
    public float waitTime = 0; // After reaching position
    public PathEvent[] events;

    public PathPoint(Vector3 position)
    {
        this.position = position;
        this.events = new PathEvent[0];
    }

    public PathPoint(Vector3 position, float approachSpeed) : this(position)
    {
        this.approachSpeed = approachSpeed;
    }

    public PathPoint(Vector3 position, Quaternion rotation, float approachSpeed) : this(position, approachSpeed)
    {
        this.rotation = rotation;
        this.affectRotation = true;
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