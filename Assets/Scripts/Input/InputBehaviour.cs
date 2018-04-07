using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBehaviour : MonoBehaviour
{
    [SerializeField]
    protected InputReceiver input;

    protected virtual void Reset()
    {
        input = GetComponent<RewiredInputReceiver>();
    }

    protected virtual void Awake()
    {
        if (input == null)
            input = GetComponentInParent<InputReceiver>();

        if (input == null)
            Debug.LogWarning(Warnings.NoComponentSpecified(this, typeof(InputReceiver)));
    }
}
