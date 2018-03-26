using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackingAI : MonoBehaviour
{
    public Transform target;

    [Header("Parameters")]
    public bool lookWithYAxis = false;
    public Vector3 constantAxis = Vector3.up;
    public float turnLerp = 1f;

    void Update()
    {
        Vector3 movement = target.position - transform.position;

        if (target != null)
        {
            Quaternion targetRotation = lookWithYAxis ? Quaternion.LookRotation(constantAxis, movement) : Quaternion.LookRotation(movement, constantAxis);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnLerp);
        }
    }
}
