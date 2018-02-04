using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionByInput : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputReceiver input;
    public string axisPairName = "Move";
    [Space]

    public bool restrictToOrthogonal = true;
    public bool allowZeroDistance = false;
    public float minDistance = 1f;
    public float maxDistance = 1f;

    private void Awake()
    {
        if (input == null)
        {
            Debug.LogWarning(Warnings.RequireComponent(this));
            enabled = false;
        }
    }

    private void Update()
    {
        Vector2 movement = restrictToOrthogonal ? input.GetAxisPairSingle(axisPairName) : input.GetAxisPair(axisPairName);
        if (allowZeroDistance == false && movement == Vector2.zero)
        {
            return;
        }
        movement = movement.normalized * Mathf.Lerp(minDistance, maxDistance, (movement.magnitude - input.defaultDeadZone) / (1 - input.defaultDeadZone));
        transform.localPosition = movement;
    }
}
