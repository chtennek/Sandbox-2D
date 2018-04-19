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
        if (input == null)
        {
            Warnings.ComponentMissing<InputReceiver>(this);
            enabled = false;
        }
    }

    protected override void Update()
    {
        if (input.GetButtonDown(buttonName))
            Active = true;
        if (input.GetButtonUp(buttonName))
            Active = false;

        base.Update();
    }
}
