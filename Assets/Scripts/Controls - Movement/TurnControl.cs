using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControl : InputBehaviour
{
    [Header("Input")]
    public string axisPairName = "Aim";
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;

    [Header("Parameters")]
    public RotationSettings rotator;
    public float turnLerp = 1f;

    private void FixedUpdate()
    {
        Vector3 movement = input.GetAxisPair(axisPairName);
        movement = Grid.Swizzle(swizzle, movement);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = rotator.GetRotationTowards(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnLerp);
        }
    }
}
