using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGrabControl : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputReceiver input;
    [SerializeField]
    private string buttonName = "Fire";

    [Header("References")]
    [SerializeField]
    private RigidbodyWrapper mover;
    [SerializeField]
    private GridMovement grid;
    [SerializeField]
    private CollideTrigger coll;

    public GridMovement currentlyHeld;

    protected void Reset()
    {
        input = GetComponent<InputReceiver>();
        mover = GetComponent<RigidbodyWrapper>();
        grid = GetComponent<GridMovement>();
        coll = GetComponent<CollideTrigger>();
    }

    protected void Awake()
    {
        if (input == null)
            Warnings.ComponentMissing<InputReceiver>(this);
        
        if (mover == null || grid == null || coll == null)
            Warnings.ComponentMissing(this);
    }

    public void Grab()
    {
        if (currentlyHeld != null)
            return;

        Transform t = coll.Target;
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
