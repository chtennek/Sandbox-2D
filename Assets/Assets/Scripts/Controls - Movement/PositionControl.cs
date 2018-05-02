using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public string axisPairName = "Move";

    [Header("Parameters")]
    public bool restrictToOrthogonal = true;
    public bool allowZeroDistance = false;
    public float minDistance = 1f;
    public float maxDistance = 1f;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        if (input == null)
            Warnings.ComponentMissing(this);
    }

    private void Update()
    {
        if (input == null)
            return;
        
        Vector2 movement = input.GetAxisPair(axisPairName);
        if (allowZeroDistance == false && movement == Vector2.zero)
            return;

        if (restrictToOrthogonal)
            movement = movement.LargestAxis();
        
        movement = movement.normalized * Mathf.Lerp(minDistance, maxDistance, (movement.magnitude - input.deadZone) / (1 - input.deadZone));

        transform.localPosition = movement;
    }
}
