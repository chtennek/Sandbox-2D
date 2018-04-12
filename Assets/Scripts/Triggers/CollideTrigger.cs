using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollideTrigger : ContextTrigger
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private string[] tagMask;
    [SerializeField] private bool ignoreSiblings = true;

    private bool TransformMask(Transform other)
    {
        if (ignoreSiblings && other.parent != null && transform.parent == other.transform.parent)
            return false;
        if (mask.Contains(other.gameObject.layer) == false)
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

    private void OnTriggerEnter(Collider collision)
    {
        Transform other = collision.transform;
        if (TransformMask(other) == false)
            return;

        CollideOn(other);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform other = collision.transform;
        if (TransformMask(other) == false)
            return;

        CollideOn(other);
    }

    private void OnTriggerExit(Collider collision)
    {
        Transform other = collision.transform;
        if (TransformMask(other) == false)
            return;

        CollideOff(other);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Transform other = collision.transform;
        if (TransformMask(other) == false)
            return;

        CollideOff(other);
    }
}
