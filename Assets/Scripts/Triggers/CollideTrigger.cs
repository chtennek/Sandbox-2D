using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollideTrigger : Trigger
{
    public LayerMask mask;
    public bool ignoreSiblings = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreSiblings && transform.parent != null && transform.parent == collision.transform.parent)
            return;
        if (mask.Contains(collision.gameObject.layer) == false)
            return;

        onActivate.Invoke();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (ignoreSiblings && transform.parent != null && transform.parent == collision.transform.parent)
            return;
        if (mask.Contains(collision.gameObject.layer) == false)
            return;

        onDeactivate.Invoke();
    }

    protected override bool Check()
    {
        return false;
    }
}
