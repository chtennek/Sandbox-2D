using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollideTrigger : Trigger
{
    public LayerMask mask;
    public string tagName;
    public bool ignoreSiblings = true;

    private bool FilterTransform(Transform t) {
        if (ignoreSiblings && t.parent != null && transform.parent == t.transform.parent)
            return false;
        if (mask.Contains(t.gameObject.layer) == false)
            return false;
        if (tagName.Length > 0 && t.tag != tagName)
            return false;
        return true;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (FilterTransform(collision.transform) == false)
            return;
        Debug.Log(collision.gameObject.name + " Enter");
        Active = true; // [TODO] keep list of overlapping triggers to avoid weird behavior
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (FilterTransform(collision.transform) == false)
            return;
        Active = true;
    }

    public void OnTriggerExit(Collider collision)
    {
        if (FilterTransform(collision.transform) == false)
            return;
        Debug.Log(collision.gameObject.name + " Exit");
        Active = false;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (FilterTransform(collision.transform) == false)
            return;
        Active = false;
    }
}
