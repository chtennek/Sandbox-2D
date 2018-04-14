using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class SpawnTrigger : Trigger
{
    public LayerMask mask;
    private Collider coll;
    private GridMovement gridMovement;

    private void Awake()
    {
        coll = GetComponent<Collider>();
        gridMovement = GetComponent<GridMovement>();
    }

	protected override void Update()
	{
        Active = Physics.CheckBox(transform.position, coll.bounds.extents, Quaternion.identity, mask) == false && gridMovement.IsPushableTowards(Vector3.down);
		base.Update();
	}
}
