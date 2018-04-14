using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Rotator
{
    public bool lookWithYAxis;
    public Vector3 constantAxis;

    public Rotator(bool lookWithYAxis, Vector3 constantAxis)
    {
        this.lookWithYAxis = lookWithYAxis;
        this.constantAxis = constantAxis;
    }

    public Quaternion GetRotationTowards(Vector3 target)
    {
        if (constantAxis == Vector3.zero)
            constantAxis = Vector3.up;
        Quaternion targetRotation = lookWithYAxis ? Quaternion.LookRotation(constantAxis, target) : Quaternion.LookRotation(target, constantAxis);
        return targetRotation;
    }
}
