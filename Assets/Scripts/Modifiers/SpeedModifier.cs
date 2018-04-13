using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : Modifier {
    [SerializeField] private MoveControl control;

	private void Reset()
	{
        control = GetComponentInParent<MoveControl>();
	}

	private void Awake()
	{
        if (control != null)
            BaseValue = control.walkSpeed;
	}

	protected override float CurrentValue
	{
		get
		{
            return control.walkSpeed;
		}

		set
		{
            control.walkSpeed = value;
		}
	}
}
