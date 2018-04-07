using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGrabControl : InputBehaviour
{
    [SerializeField] private string buttonName = "Fire";

    [SerializeField] private GridMovement grid;
    [SerializeField] private CollideTrigger coll;

    public GridMovement currentlyHeld;
    private RigidbodyWrapper mover;
    private Rigidbody rb;

    protected override void Reset()
    {
        base.Reset();
        grid = GetComponent<GridMovement>();
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
        if (currentlyHeld != null)
            return;

        Transform t = coll.GetCollision();
        if (t == null) return;

        currentlyHeld = t.GetComponentInParent<GridMovement>();
        grid.AddObject(currentlyHeld);
    }

    public void Release()
    {
        if (currentlyHeld == null)
            return;

        grid.RemoveObject(currentlyHeld);
        currentlyHeld = null;
    }

    private void Update()
    {
        bool buttonHeld = input.GetButton(buttonName);

        if (buttonHeld == false && grid.IsMoving == false)
        {
            Release();
        }
        else if (buttonHeld == true && grid.Pushable == true)
        {
            Grab();
        }
    }
}
