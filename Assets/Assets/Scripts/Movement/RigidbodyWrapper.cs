using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyWrapper : MonoBehaviour
{
    public ColliderChecker colliderCheck; // [TODO] move this

    [SerializeField]
    private Vector3 m_velocity = Vector3.zero;

    private HashSet<Transform> grounds = new HashSet<Transform>();
    private HashSet<GravityField> fields = new HashSet<GravityField>();

    private Rigidbody rb;
    private Rigidbody2D rb2D;
    private Collider coll;
    private Collider2D coll2D;

    public Vector3 Velocity
    {
        get
        {
            if (rb != null) return rb.velocity;
            else if (rb2D != null) return rb2D.velocity;
            return m_velocity;
        }
        set
        {
            if (rb != null) rb.velocity = value;
            else if (rb2D != null) rb2D.velocity = value;
            m_velocity = value;
        }
    }

    public float Speed
    {
        get
        {
            return Velocity.magnitude;
        }
        set
        {
            Velocity = value * Velocity.normalized;
        }
    }

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb2D = GetComponentInParent<Rigidbody2D>();
        coll = GetComponent<Collider>();
        coll2D = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if ((rb == null && rb2D == null) ||
            (rb != null && rb.isKinematic == true) ||
            (rb2D != null && rb2D.isKinematic == true))
            transform.position += Velocity * Time.fixedDeltaTime;

        AddForce(GetTotalField());
    }

    private void OnCollisionEnter(Collision collision)
    {
		UnityEngine.ContactPoint[] contacts = collision.contacts;
        foreach (UnityEngine.ContactPoint contact in contacts)
        {
            if (contact.normal == Vector3.up)
            {
				grounds.Add(contact.otherCollider.transform);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        foreach (ContactPoint2D contact in contacts)
        {
            if (contact.normal == Vector2.up)
            {
                grounds.Add(contact.otherCollider.transform);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounds.Remove(collision.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounds.Remove(collision.transform);
    }

    public bool IsGrounded()
    {
        return grounds.Count > 0;
    }

    public void AddForce(Vector3 force) { AddForce(force, ForceMode2D.Force); }
    public void AddForce(Vector3 force, ForceMode2D mode)
    {
        if (rb != null && rb.isKinematic == false)
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
        else if (rb2D != null && rb2D.isKinematic == false)
        {
            rb2D.AddForce(force, mode);
        }
        else
        {
            switch (mode)
            {
                case ForceMode2D.Force:
                    Velocity += force * Time.deltaTime;
                    break;
                case ForceMode2D.Impulse:
                    Velocity += force;
                    break;
            }
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

    private void AddField(GravityField field)
    {
        if (field != null && field.InFilter(gameObject.layer))
            fields.Add(field);
    }

    private void RemoveField(GravityField field)
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
    public float maxAngle = 10f;
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