using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollideTrigger : Trigger
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private string tagName;
    [SerializeField] private bool ignoreSiblings = true;

    private List<Transform> collidingWith = new List<Transform>(); // [TODO] keep list of colliding triggers to avoid weird behavior
    public Transform Other { get { return collidingWith.Count == 0 ? null : collidingWith[0]; } }

    private bool FilterTransform(Transform t)
    {
        if (ignoreSiblings && t.parent != null && transform.parent == t.transform.parent)
            return false;
        if (mask.Contains(t.gameObject.layer) == false)
            return false;
        if (tagName.Length > 0 && t.tag != tagName)
            return false;
        return true;
    }

    public Transform GetCollision() { return GetCollision(0); }
    public Transform GetCollision(int index) { return collidingWith.Count <= index ? null : collidingWith[index]; }

    public void CollideOn(Transform transform)
    {
        collidingWith.Add(transform);
        Active = true;
    }

    public void CollideOff(Transform transform)
    {
        collidingWith.Remove(transform);
        Active = false;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (FilterTransform(collision.transform) == false)
            return;
        collidingWith.Add(collision.transform);
        Active = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (FilterTransform(collision.transform) == false)
            return;
        collidingWith.Add(collision.transform);
        Active = true;
    }

    public void OnTriggerExit(Collider collision)
    {
        if (FilterTransform(collision.transform) == false)
            return;
        collidingWith.Remove(collision.transform);
        Active = false;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (FilterTransform(collision.transform) == false)
            return;
        collidingWith.Remove(collision.transform);
        Active = false;
    }
}
