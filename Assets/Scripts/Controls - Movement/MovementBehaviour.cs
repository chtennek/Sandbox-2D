using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    public GroundedCheckOptions options;

    [SerializeField]
    protected InputReceiver input;

    private RaycastHit2D[] results = new RaycastHit2D[5];
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
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();

        if (input == null)
            input = GetComponent<InputReceiver>();

        if (input == null)
        {
            Warnings.NoComponentSpecified(this, input);
            enabled = false;
        }
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
        if (coll2D != null) return 0 < coll2D.Cast(options.groundedDirection, options.contactFilter, results, options.checkDistance);
        return false;
    }
}

[System.Serializable]
public class GroundedCheckOptions
{
    public Vector3 groundedDirection = Vector3.down;
    public float checkDistance = 0.01f;
    public ContactFilter2D contactFilter;
}