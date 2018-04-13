using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollideTrigger : ContextTrigger
{
    public LayerMask layerMask = ~0;
    public string[] tagMask;
    public bool ignoreSiblings = true;
    public bool preferRigidbody = true;

    private bool TransformMask(Transform other)
    {
        if (ignoreSiblings && other.parent != null && transform.parent == other.transform.parent)
            return false;
        if (layerMask.Contains(other.gameObject.layer) == false)
            return false;

        if (tagMask.Length == 0)
            return true;
        for (int i = 0; i < tagMask.Length; i++)
        {
            if (other.tag == tagMask[i])
                return true;
        }

        return false;
    }

    public void DestroyObject(Transform other)
    {
        Destroy(other.gameObject);
    }

    private void CollideOn(Transform other)
    {
        AddTarget(other);
        Active = true;
    }

    private void CollideOff(Transform other)
    {
        RemoveTarget(other);
        Active = false;
    }

    private Transform FindRigidbody(Transform other, Collider coll = null, Collider2D coll2D = null)
    {
        if (preferRigidbody == true)
        {
            if (coll != null && coll.attachedRigidbody != null)
                return coll.attachedRigidbody.transform;

            if (coll2D != null && coll2D.attachedRigidbody != null)
                return coll2D.attachedRigidbody.transform;
        }
        return other;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Transform other = FindRigidbody(collision.transform, coll: collision);

        if (TransformMask(other) == false)
            return;

        CollideOn(other);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform other = FindRigidbody(collision.transform, coll2D: collision);

        if (TransformMask(other) == false)
            return;

        CollideOn(other);
    }

    private void OnTriggerExit(Collider collision)
    {
        Transform other = FindRigidbody(collision.transform, coll: collision);

        if (TransformMask(other) == false)
            return;

        CollideOff(other);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Transform other = FindRigidbody(collision.transform, coll2D: collision);

        if (TransformMask(other) == false)
            return;

        CollideOff(other);
    }
}
