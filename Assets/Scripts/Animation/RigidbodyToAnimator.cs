using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyToAnimator : MonoBehaviour
{
    [Header("Parameters")]
    public string xVelocityParameter = "xVelocity";
    public string yVelocityParameter = "yVelocity";
    public string gravityScaleParameter = "gravityScale";

    private List<string> parameters;

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            enabled = false;
        }

        parameters = new List<string>();
        foreach (AnimatorControllerParameter p in anim.parameters)
        {
            parameters.Add(p.name);
        }

    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (parameters.Contains(xVelocityParameter))
            anim.SetFloat(xVelocityParameter, rb.velocity.x);

        if (parameters.Contains(yVelocityParameter))
            anim.SetFloat(yVelocityParameter, rb.velocity.y);

        if (parameters.Contains(gravityScaleParameter))
            anim.SetFloat(gravityScaleParameter, rb.gravityScale);
    }
}
