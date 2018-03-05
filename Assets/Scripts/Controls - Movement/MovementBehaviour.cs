using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : InputBehaviour
{
    public ColliderChecker colliderCheck; // [TODO] move this

    private Rigidbody rb;
    private Rigidbody2D rb2D;
    private Collider2D coll2D;

    public Vector3 velocity
    {
        get
        {
            if (rb != null) return rb.velocity;
            if (rb2D != null) return rb2D.velocity;
            return Vector3.zero;
        }
        set
        {
            if (rb != null) rb.velocity = value;
            if (rb2D != null) rb2D.velocity = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        rb2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
    }

    protected void AddForce(Vector3 force) { AddForce(force, ForceMode2D.Force); }
    protected void AddForce(Vector3 force, ForceMode2D mode)
    {
        if (rb != null)
        {
            switch (mode)
            {
                case ForceMode2D.Force:
                    rb.AddForce(force, ForceMode.Force);
                    break;
                case ForceMode2D.Impulse:
                    rb.AddForce(force, ForceMode.Impulse);
                    break;
            }
        }
        if (rb2D != null)
            rb2D.AddForce(force, mode);
    }

    public bool IsGrounded()
    {
        // [TODO] add coll for 3D checks
        return colliderCheck.Cast(coll2D);
    }
}

[System.Serializable]
public class ColliderChecker
{
    public Vector3 direction = Vector3.down;
    public float checkDistance = 0.01f;
    public ContactFilter2D contactFilter;

    private RaycastHit2D[] results = new RaycastHit2D[5];

    public bool Cast(Collider2D coll2D)
    {
        if (coll2D == null) return false;
        return 0 < coll2D.Cast(direction, contactFilter, results, checkDistance);
    }

    public bool Raycast(Collider2D coll2D)
    {
        Physics2D.Raycast(coll2D.transform.position, direction, contactFilter, results);
        float width = 1f + checkDistance;
        int count = coll2D.Raycast(direction, contactFilter, results, width);
        return count > 0;
    }
}