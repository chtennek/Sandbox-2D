using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public ColliderChecker colliderCheck; // [TODO] move this

    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    private HashSet<GravityField> fields = new HashSet<GravityField>();

    private Rigidbody rb;
    private Rigidbody2D rb2D;
    private Collider2D coll2D;

    public Vector3 Velocity
    {
        get
        {
            if (rb != null) return rb.velocity;
            else if (rb2D != null) return rb2D.velocity;
            return velocity;
        }
        set
        {
            if (rb != null) rb.velocity = value;
            else if (rb2D != null) rb2D.velocity = value;
            velocity = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        AddForce(GetTotalField());
        if (rb == null && rb2D == null)
            transform.position += Velocity * Time.fixedDeltaTime;
    }

    public bool IsGrounded()
    {
        // [TODO] add coll for 3D checks
        return colliderCheck.Cast(coll2D);
    }

    public void AddForce(Vector3 force) { AddForce(force, ForceMode2D.Force); }
    public void AddForce(Vector3 force, ForceMode2D mode)
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
        else if (rb2D != null)
        {
            rb2D.AddForce(force, mode);
        }
    }

    public Vector3 GetTotalField()
    {
        Vector3 total = Vector3.zero;
        foreach (GravityField field in fields)
        {
            total += field.GetForce(transform);
        }
        return total;
    }

    public void AddField(GravityField field)
    {
        if (field != null && field.InFilter(gameObject.layer))
            fields.Add(field);
    }

    public void RemoveField(GravityField field)
    {
        fields.Remove(field);
    }

    private void OnTriggerEnter(Collider col)
    {
        AddField(col.GetComponent<GravityField>());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        AddField(col.GetComponent<GravityField>());
    }

    private void OnTriggerExit(Collider col)
    {
        RemoveField(col.GetComponent<GravityField>());
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        RemoveField(col.GetComponent<GravityField>());
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