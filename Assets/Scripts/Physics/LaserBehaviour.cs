using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    private LineRenderer[] lines;

    private void Awake()
    {
        lines = GetComponentsInChildren<LineRenderer>();
    }
}
