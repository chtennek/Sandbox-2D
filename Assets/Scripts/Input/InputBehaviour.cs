using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBehaviour : MonoBehaviour {
    [SerializeField]
    protected InputReceiver input;

    protected virtual void Awake() {
        if (input == null)
            input = GetComponentInParent<InputReceiver>();

        if (input == null)
        {
            Warnings.NoComponentSpecified(this, input);
            enabled = false;
        }
    }
}
