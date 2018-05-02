using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControl : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public string axisPairName = "Aim";
    public bool restrictToXAxis = true;
    public bool restrictToYAxis = true;
    public GridLayout.CellSwizzle swizzle = GridLayout.CellSwizzle.XYZ;

    [Header("Parameters")]
    public Rotator rotator;
    public float turnLerp = 1f;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        if (input == null)
            Warnings.ComponentMissing(this);
    }

    private void FixedUpdate()
    {
        if (input == null)
            return;
        
        Vector3 movement = input.GetAxisPair(axisPairName);
        movement = Grid.Swizzle(swizzle, movement);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = rotator.GetRotationTowards(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnLerp);
        }
    }
}
