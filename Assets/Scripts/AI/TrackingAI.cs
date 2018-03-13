using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackingAI : MonoBehaviour
{
    public Transform target;
    public Vector3 front = Vector3.forward;

    void Update()
    {
        if (target != null)
        {
            transform.rotation = Quaternion.FromToRotation(front, target.position - transform.position);
        }
    }
}
