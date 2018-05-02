using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorBase : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    protected string xParameter = "xVelocity";
    [SerializeField]
    protected string yParameter = "yVelocity";

    protected List<string> parameters;
    protected Animator anim;

    protected abstract void UpdateAnimationParameters();

    protected virtual void Awake()
    {
        Setup();
    }

    protected void Setup()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning(name + ": Required components not attached!");
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
        UpdateAnimationParameters();
    }
}
