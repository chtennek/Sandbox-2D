using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollideTrigger : Trigger
{
    public CompoundMask mask;
    public bool preferRigidbody = true;

    private void CollideOn(Transform other)
    {
        if (other == null)
            return;
        Set(other);
    }

    private void CollideOff(Transform other)
    {
        if (other == null)
            return;
        Unset(other);
    }

    private Transform GetTransform(Transform other, Collider coll = null, Collider2D coll2D = null)
    {
        Transform target = other;

        if (preferRigidbody == true)
        {
            if (coll != null && coll.attachedRigidbody != null)
                target = coll.attachedRigidbody.transform;

            if (coll2D != null && coll2D.attachedRigidbody != null)
                target = coll2D.attachedRigidbody.transform;
        }

        if (mask.Check(target, transform) == false)
            return null;

        return target;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform other = GetTransform(collision.transform, coll: collision.collider);
        CollideOn(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform other = GetTransform(collision.transform, coll2D: collision.collider);
        CollideOn(other);
    }

    private void OnCollisionExit(Collision collision)
    {
        Transform other = GetTransform(collision.transform, coll: collision.collider);
        CollideOff(other);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Transform other = GetTransform(collision.transform, coll2D: collision.collider);
        CollideOff(other);
    }
}
