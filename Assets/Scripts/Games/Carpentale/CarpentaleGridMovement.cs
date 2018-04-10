using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpentaleGridMovement : GridMovement
{
    [Header("Merging")]
    public Transform mergeEffectPrefab;
    public string mergeObjectsWithTag = "Block";
    public float mergeTime = .5f;
    public float mergeContributionWindow = 1f;

    private float mergeStartTime;
    private HashSet<Transform> mergeablesLastFrame;

    public override bool Move(Vector3 v, bool fixRotation)
    {
        if (IsMoving == true)
            return false;

        Vector3 target = FindNearestGridPoint(transform.position + Vector3.Scale(gridScale, v));
        Vector3 movement = target - transform.position;

        HashSet<GridMovement> pushables = GetAffectedObjectsAlong(movement, false);
        HashSet<GridMovement> allAffected = GetAffectedObjectsAlong(movement, true);

        // Check if there's something in the way
        if (pushables.SetEquals(allAffected))
        {
            mergeablesLastFrame = null;
            foreach (GridMovement g in pushables)
                g.Push(movement, fixRotation);
            return true;
        }

        HashSet<Transform> mergeables = FindMergeables(allAffected);
        if (mergeablesLastFrame == null || mergeables.SetEquals(mergeablesLastFrame) == false)
        {
            mergeablesLastFrame = mergeables;
            mergeStartTime = Time.time;
        }
        else if (Time.time - mergeStartTime >= mergeTime)
        {
            SpawnEffectsAtContactPoints(mergeables);
            MergeObjects(mergeables);
        }
        return false;
    }

    private HashSet<Transform> FindMergeables(HashSet<GridMovement> objects)
    {
        HashSet<Transform> mergeables = new HashSet<Transform>();
        foreach (GridMovement obj in objects)
        {
            if (obj.gameObject.tag == mergeObjectsWithTag)
                mergeables.Add(obj.transform);
        }
        return mergeables;
    }

    // [TODO] performance
    private void SpawnEffectsAtContactPoints(HashSet<Transform> mergeables)
    {
        if (mergeEffectPrefab == null)
            return;

        List<Vector3> cubePoints = new List<Vector3>();
        foreach (Transform container in mergeables)
        {
            foreach (Transform child in container)
            {
                foreach (Vector3 point in cubePoints)
                {
                    if (Vector3.Distance(point, child.position) == 1)
                    {
                        Vector3 position = .5f * (point + child.position);
                        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, point - child.position);
                        Instantiate(mergeEffectPrefab, position, rotation);
                    }
                }
            }
            foreach (Transform child in container)
            {
                cubePoints.Add(child.position);
            }
        }
    }

    private void MergeObjects(HashSet<Transform> mergeables)
    {

        List<Transform> children = new List<Transform>();
        Transform parent = transform;
        foreach (Transform mergeable in mergeables)
        {
            if (parent == transform)
            {
                parent = mergeable;
                continue;
            }

            children.Clear();
            foreach (Transform child in mergeable)
                children.Add(child);
            foreach (Transform child in children)
                child.parent = parent;
            Destroy(mergeable.gameObject);
        }
    }
}
