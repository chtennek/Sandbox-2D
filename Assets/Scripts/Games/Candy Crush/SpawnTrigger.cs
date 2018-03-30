using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class SpawnTrigger : Trigger {
    private GridMovement gridMovement;

	private void Awake()
	{
        gridMovement = GetComponent<GridMovement>();
	}

	protected override bool ConditionsMet() {
        return gridMovement.IsPushableTowards(Vector3.down);
    }
}
