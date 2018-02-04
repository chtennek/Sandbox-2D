using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputReceiver))]
public class InputToAnimator : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputReceiver input;
    public string axisPairName = "Move";

    [Header("Parameters")]
    [SerializeField]
    private string xParameter = "xVelocity";
    [SerializeField]
    private string yParameter = "yVelocity";

    private List<string> parameters;

    private Animator anim;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        anim = GetComponent<Animator>();
        if (input == null || anim == null)
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
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector2 movement = input.GetAxisPairSingle(axisPairName).normalized;

        if (parameters.Contains(xParameter))
            anim.SetFloat(xParameter, movement.x);

        if (parameters.Contains(yParameter))
            anim.SetFloat(yParameter, movement.y);
    }
}
