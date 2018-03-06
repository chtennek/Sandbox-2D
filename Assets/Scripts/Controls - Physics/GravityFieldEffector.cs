using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldEffector : MovementBehaviour
{
    private HashSet<GravityField> fields = new HashSet<GravityField>();

    public Vector3 GetTotalField()
    {
        Vector3 total = Vector3.zero;
        foreach (GravityField field in fields)
        {
            total += field.GetForce(transform);
        }
        return total;
    }

    private void FixedUpdate()
    {
        AddForce(GetTotalField());
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
