using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationMethod
{
    Input,
    Velocity,
}

public class AnimatorMessenger : MonoBehaviour
{
    public AnimationMethod mode;

    [Header("Parameters")]
    public string xParameter = "xVelocity";
    public string yParameter = "yVelocity";
    public string speedParameter = "speed";
    public float multiply = 1;

    [Header("References")]
    public string axisPairName;
    public InputReceiver input;
    public RigidbodyWrapper rb;
    public Animator animator;

    private List<string> parameters;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
        rb = GetComponent<RigidbodyWrapper>();
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        if (animator == null)
        {
            Warnings.ComponentMissing(this);
            return;
        }

        parameters = new List<string>();
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            parameters.Add(p.name);
        }
    }

    private void Update()
    {
        Vector3 direction = GetDirection();

        if (parameters.Contains(xParameter))
            animator.SetFloat(xParameter, direction.x);

        if (parameters.Contains(yParameter))
            animator.SetFloat(yParameter, direction.y);

        if (parameters.Contains(speedParameter))
            animator.SetFloat(speedParameter, direction.magnitude);
    }

    private Vector3 GetDirection()
    {
        switch (mode)
        {
            case AnimationMethod.Input:
                if (input != null)
                    return multiply * input.GetAxisPair(axisPairName);
                break;
            case AnimationMethod.Velocity:
                if (rb != null)
                    return multiply * rb.Velocity;
                break;
        }
        return Vector3.zero;
    }
}
