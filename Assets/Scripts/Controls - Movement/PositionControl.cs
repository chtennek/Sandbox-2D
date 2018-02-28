using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MovementBehaviour
{
    public string axisPairName = "Move";

    [Header("Parameters")]
    public bool restrictToOrthogonal = true;
    public bool allowZeroDistance = false;
    public float minDistance = 1f;
    public float maxDistance = 1f;

    private void Update()
    {
        Vector2 movement = restrictToOrthogonal ? input.GetAxisPairSingle(axisPairName) : input.GetAxisPair(axisPairName);
        if (allowZeroDistance == false && movement == Vector2.zero)
        {
            return;
        }
        movement = movement.normalized * Mathf.Lerp(minDistance, maxDistance, (movement.magnitude - input.deadZone) / (1 - input.deadZone));
        transform.localPosition = movement;
    }
}
