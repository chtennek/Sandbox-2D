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
    public AnimationMethod mode = AnimationMethod.Velocity;

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
        if (animator == null)
            Warnings.ComponentMissing(this);
        else
        {
            parameters = new List<string>();
            foreach (AnimatorControllerParameter p in animator.parameters)
                parameters.Add(p.name);
        }
    }

    private void Update()
    {
        Vector3 direction = GetDirection();
        SetFloat(xParameter, direction.x);
        SetFloat(yParameter, direction.y);
        SetFloat(speedParameter, direction.magnitude);
    }

    public void SetFloat(string parameter, float value)
    {
        if (parameters.Contains(parameter))
            animator.SetFloat(parameter, value);
    }

    public void SetInteger(string parameter, int value)
    {
        if (parameters.Contains(parameter))
            animator.SetInteger(parameter, value);
    }

    public void SetBool(string parameter, bool value)
    {
        if (parameters.Contains(parameter))
            animator.SetBool(parameter, value);
    }

    public void SetTrigger(string parameter)
    {
        if (parameters.Contains(parameter))
            animator.SetTrigger(parameter);
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
