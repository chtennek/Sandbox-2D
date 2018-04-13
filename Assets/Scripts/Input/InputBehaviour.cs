﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBehaviour : MonoBehaviour
{
    [SerializeField]
    protected InputReceiver input;

    protected virtual void Reset()
    {
        input = GetComponentInParent<InputReceiver>();
    }

    protected virtual void Awake()
    {
        if (input == null)
            Warnings.ComponentMissing<InputReceiver>(this);
    }
}
