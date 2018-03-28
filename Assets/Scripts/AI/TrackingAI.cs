using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackingAI : MonoBehaviour
{
    public Transform target;

    [Header("Parameters")]
    public Rotator rotator;
    public float turnLerp = 1f;

    void Update()
    {
        Vector3 movement = target.position - transform.position;

        if (target != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotator.GetRotationTowards(movement), turnLerp);
        }
    }
}
