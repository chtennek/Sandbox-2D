using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSummation : MovementBehaviour
{
    public Vector3 defaultField = 9.81f * Vector3.down;

    public void FixedUpdate()
    {
        AddForce(defaultField);
    }

    public Vector3 GetTotalField()
    {
        return defaultField;
    }
}
