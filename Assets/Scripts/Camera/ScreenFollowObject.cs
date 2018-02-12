using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ScreenFollowObject : MonoBehaviour
{
    public Transform[] objects;
    public float lerpValue = 0.5f;
    public float allowableOffset = 0f;

    private void Update()
    {
        Vector3 target = CalculateCenterPoint();
        if ((transform.position - target).magnitude > allowableOffset)
        {
            transform.position = Vector3.Lerp(transform.position, target, lerpValue);
        }
    }

    private Vector3 CalculateCenterPoint()
    {
        if (objects.Length == 0)
        {
            return Vector3.zero;
        }

        Vector3 sum = Vector3.zero;
        foreach (Transform t in objects)
        {
            if (t != null)
            {
                sum += t.position;
            }
        }

        sum.z = transform.position.z; // Keep the camera at a fixed depth
        return sum / objects.Length;
    }
}
