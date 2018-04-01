using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackingAI : MonoBehaviour
{
    public Transform target;
    public string targetByTag;

    [Header("Parameters")]
    public Rotator rotator;
    public float turnLerp = 1f;

    private void Awake()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag(targetByTag).transform;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 movement = target.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotator.GetRotationTowards(movement), turnLerp);
        }
    }
}
