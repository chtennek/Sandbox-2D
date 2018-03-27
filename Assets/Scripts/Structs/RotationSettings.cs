using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RotationSettings
{
    public bool lookWithYAxis;
    public Vector3 constantAxis;

    public RotationSettings(bool lookWithYAxis, Vector3 constantAxis)
    {
        this.lookWithYAxis = lookWithYAxis;
        this.constantAxis = constantAxis;
    }

    public Quaternion GetRotationTowards(Vector3 target)
    {
        Quaternion targetRotation = lookWithYAxis ? Quaternion.LookRotation(constantAxis, target) : Quaternion.LookRotation(target, constantAxis);
        return targetRotation;
    }
}
