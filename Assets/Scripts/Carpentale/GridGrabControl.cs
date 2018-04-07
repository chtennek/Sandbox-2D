using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGrabControl : InputBehaviour
{
    [SerializeField] private string buttonName = "Fire";

    [SerializeField] private PathControl path;
    [SerializeField] private GridControl grid;
    [SerializeField] private CollideTrigger coll;

    private RigidbodyWrapper mover;
    private Rigidbody rb;

    protected override void Reset()
    {
        base.Reset();
        path = GetComponent<PathControl>();
        grid = GetComponent<GridControl>();
        coll = GetComponent<CollideTrigger>();
    }

    protected override void Awake()
    {
        base.Awake();
        mover = GetComponent<RigidbodyWrapper>();
        rb = GetComponent<Rigidbody>();
    }

    public void Grab()
    {
    }

    public void Release()
    {
    }

    private void Update()
    {
        bool buttonHeld = input.GetButton(buttonName);
        if (path.Count > 0) return;
    }
}
