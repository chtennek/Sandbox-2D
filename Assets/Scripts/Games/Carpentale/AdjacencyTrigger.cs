using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacencyTrigger : Trigger
{
    [SerializeField] private Transform other;

    [SerializeField] private GridMovement grid;

    private void Reset()
    {
        grid = GetComponent<GridMovement>();
    }

    private void Awake()
    {
        if (other == null || grid == null)
        {
            Warnings.ComponentMissing(this);
            enabled = false;
        }
    }

    protected override void Update()
    {
        Active = Vector3.Distance(grid.FindNearestGridPoint(transform.position), grid.FindNearestGridPoint(other.position)) <= 1;
        base.Update();
    }
}
