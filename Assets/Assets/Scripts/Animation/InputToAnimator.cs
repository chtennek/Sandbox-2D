using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputReceiver))]
public class InputToAnimator : AnimatorBase
{
    [Header("Input")]
    [SerializeField]
    private InputReceiver input;
    public string axisPairName = "Move";

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<InputReceiver>();
    }

    protected override void UpdateAnimation()
    {
        Vector2 movement = input.GetAxisPairSingle(axisPairName).normalized;

        if (parameters.Contains(xParameter))
            anim.SetFloat(xParameter, movement.x);

        if (parameters.Contains(yParameter))
            anim.SetFloat(yParameter, movement.y);
    }
}
