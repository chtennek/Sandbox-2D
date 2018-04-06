using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : Trigger
{
    [SerializeField]
    private InputReceiver input;
    public string buttonName;

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

	private void Awake()
	{
        if (input == null) {
            Debug.LogWarning(Warnings.NoComponentSpecified(this, typeof(InputReceiver)));
            enabled = false;
        }
	}

	protected override void Update()
	{
        if (input.GetButtonDown(buttonName))
            Active = true;
        else if (input.GetButtonUp(buttonName))
            Active = false;

        base.Update();
	}
}
