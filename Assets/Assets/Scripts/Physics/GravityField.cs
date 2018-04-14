using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    public LayerMask filter;

    [SerializeField]
    private Vector3 force = 9.81f * Vector3.down;
    //public bool requireObjectCenterInside = false;

    public bool InFilter(int layer)
    {
        return (filter.value & (1 << layer)) > 0;
    }

    public Vector3 GetForce(Transform target)
    {
        return force;
    }
}
