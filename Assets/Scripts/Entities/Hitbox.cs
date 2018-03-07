using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent onHit;
    public LayerMask targetLayers;
    public bool ignoreSiblings = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreSiblings && transform.parent != null && transform.parent == collision.transform.parent)
            return;
        if (targetLayers.Contains(collision.gameObject.layer) == false)
            return;

        OnHit();
    }

    public void OnHit()
    {
        onHit.Invoke();
    }
}
