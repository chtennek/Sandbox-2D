using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityFieldEffector))]
public class JumpEffector : MovementBehaviour
{
    [Header("Input")]
    public string buttonName = "Jump";

    [Header("Parameters")]
    public Vector3 direction = Vector3.up; // Direction of jump
    public float terminalSpeed = Mathf.Infinity;
    public float fallScale = 4f;
    public float inputReleaseScale = 12f;

    private GravityFieldEffector fields;

    protected override void Awake()
    {
        base.Awake();
        fields = GetComponent<GravityFieldEffector>();
    }

    protected void FixedUpdate()
    {
        Vector3 acceleration = Vector3.Dot(fields.GetTotalField(), direction.normalized) * direction.normalized;
        float vAlongDirection = Vector3.Dot(velocity, direction.normalized);
        float scale = 1f;

        if (vAlongDirection <= 0)
            scale = fallScale;
        else if (input.GetButton(buttonName) == false)
            scale = inputReleaseScale;

        AddForce((scale - 1) * acceleration);
        if (vAlongDirection <= 0 && terminalSpeed > 0)
            ApplyDrag(scale * acceleration.magnitude / terminalSpeed);
    }

    private void ApplyDrag(float drag)
    {
        Vector2 v = Vector3.Dot(velocity, direction.normalized) * direction.normalized;
        AddForce(drag * -v);
    }
}