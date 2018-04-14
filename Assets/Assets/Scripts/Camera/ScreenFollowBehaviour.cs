using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ScreenFollowBehaviour : MonoBehaviour
{
    private List<Transform> objects = new List<Transform>();
    public float lerpValue = 0.5f;
    public float allowableOffset = 0f;

    private void Update()
    {
        Vector3 focus = CalculateCenterPoint();
        Vector3 fromFocus = transform.position - focus;
        if (fromFocus.magnitude > allowableOffset)
        {
            Vector3 target = focus + allowableOffset * fromFocus.normalized;
            transform.position = Vector3.Lerp(transform.position, target, lerpValue);
        }
    }

    public void AddObject(Transform target)
    {
        objects.Add(target);
    }

    private Vector3 CalculateCenterPoint()
    {
        objects.RemoveAll(delegate (Transform o) { return o == null; });
        if (objects.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 sum = Vector3.zero;
        foreach (Transform t in objects)
        {
            sum += t.position;
        }

        sum.z = transform.position.z; // Keep the camera at a fixed depth
        return sum / objects.Count;
    }
}
