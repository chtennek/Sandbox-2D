using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpentaleGridMovement : GridMovement {
    public string mergeObjectsWithTag = "Block";

	public override bool Move(Vector3 v, bool fixRotation)
    {
        if (IsMoving == true)
            return false;

        Vector3 target = FindNearestGridPoint(transform.position + Vector3.Scale(gridScale, v));
        Vector3 movement = target - transform.position;

        // Check if there's something in the way
        HashSet<GridMovement> pushables = GetAffectedObjectsAlong(movement, false);
        HashSet<GridMovement> allAffected = GetAffectedObjectsAlong(movement, true);

        if (pushables.SetEquals(allAffected)) {
            foreach (GridMovement g in pushables)
                g.Push(movement, fixRotation);
            return true;

        }

        MergeMatchingObjects(allAffected);
        return false;
	}

    private void MergeMatchingObjects(HashSet<GridMovement> objects) {
        List<GridMovement> mergeables = new List<GridMovement>();
        foreach (GridMovement obj in objects) {
            if (obj.gameObject.tag == mergeObjectsWithTag)
                mergeables.Add(obj);
        }

        List<Transform> children = new List<Transform>();
        for (int i = 1; i < mergeables.Count; i++) {
            children.Clear();
            foreach (Transform child in mergeables[i].transform)
                children.Add(child);
            foreach (Transform child in children)
                child.parent = mergeables[0].transform;
            Destroy(mergeables[i].transform);
        }
    }
}
