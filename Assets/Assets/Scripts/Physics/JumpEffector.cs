using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidbodyWrapper))]
public class JumpEffector : MonoBehaviour
{
    public RigidbodyWrapper mover;

    [Header("Input")]
    public InputReceiver input;
    public string buttonName = "Jump";

    [Header("Parameters")]
    public Vector3 direction = Vector3.up; // Direction of jump
    public float terminalSpeed = Mathf.Infinity;
    public float fallScale = 4f;
    public float inputReleaseScale = 12f;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
        mover = GetComponent<RigidbodyWrapper>();
    }

    private void Awake()
    {
        if (input == null || mover == null)
            Warnings.ComponentMissing(this);
    }

    protected void FixedUpdate()
    {
        if (input == null || mover == null)
            return;
        
        Vector3 acceleration = Vector3.Dot(mover.GetTotalField(), direction.normalized) * direction.normalized;
        float vAlongDirection = Vector3.Dot(mover.Velocity, direction.normalized);
        float scale = 1f;

        if (vAlongDirection <= 0)
            scale = fallScale;
        else if (input.GetButton(buttonName) == false)
            scale = inputReleaseScale;

        mover.AddForce((scale - 1) * acceleration);
        if (vAlongDirection <= 0 && terminalSpeed > 0)
            ApplyDrag(scale * acceleration.magnitude / terminalSpeed);
    }

    private void ApplyDrag(float drag)
    {
        Vector2 v = Vector3.Dot(mover.Velocity, direction.normalized) * direction.normalized;
        mover.AddForce(drag * -v);
    }
}