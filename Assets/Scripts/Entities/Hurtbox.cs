using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    public UnityEvent onHurt;
    public Vector3 requiredDirection = Vector3.zero; // [TODO]
    public LayerMask hitboxLayers;

    private ContactPoint[] contacts = new ContactPoint[1];
    private ContactPoint2D[] contacts2D = new ContactPoint2D[1];

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitboxLayers.Contains(collision.gameObject.layer) == false)
            return;
        
        Vector3 direction = collision.contacts[0].normal;
        if (requiredDirection == Vector3.zero || direction == requiredDirection)
            OnHurt();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (hitboxLayers.Contains(collision.gameObject.layer) == false)
            return;
        
        Vector3 direction = collision.contacts[0].normal;
        if (requiredDirection == Vector3.zero || direction == requiredDirection)
            OnHurt();
    }

    public void OnHurt()
    {
        onHurt.Invoke();
    }
}
