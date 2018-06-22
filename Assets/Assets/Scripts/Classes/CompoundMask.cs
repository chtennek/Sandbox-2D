using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompoundMask
{
    public LayerMask layerMask = ~0;
    public List<string> tagMask;
    public bool ignoreSiblings = false;
    public float raycastDistance = 1000f;

    public List<Transform> GetCollidersWithin(float distance, Transform self) { return GetCollidersWithin(distance, self.position, self); }
    public List<Transform> GetCollidersWithin(float distance, Vector3 position, Transform self = null)
    {
        List<Transform> targets = new List<Transform>();

        // 3D
        foreach (Collider coll in Physics.OverlapSphere(position, distance, layerMask))
        {
            Transform target = GetTarget(coll);
            if (target != null && target != self)
                targets.Add(target);
        }

        // 2D
        foreach (Collider2D coll2D in Physics2D.OverlapCircleAll(position, distance, layerMask))
        {
            Transform target = GetTarget(coll2D);
            if (target != null && target != self)
                targets.Add(target);
        }

        return targets;
    }

    public List<Vector3> Mousecast(Transform self = null)
    {
        List<Vector3> targets = new List<Vector3>();

        // 3D
        List<RaycastHit> hits = new List<RaycastHit>(Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), raycastDistance, layerMask));
        hits.OrderBy((hit) => hit.distance);
        foreach (RaycastHit hit in hits)
        {
            Transform target = GetTarget(hit.collider);
            if (target != null)
                targets.Add(hit.point);
        }

        return targets;
    }

    private Transform GetTarget(Collider coll, Transform self = null)
    {
        Transform target;

        if (coll.attachedRigidbody == null)
            target = coll.transform;
        else
            target = coll.attachedRigidbody.transform;

        if (Check(target, self))
            return target;

        return null;
    }

    private Transform GetTarget(Collider2D coll, Transform self = null)
    {
        Transform target;

        if (coll.attachedRigidbody == null)
            target = coll.transform;
        else
            target = coll.attachedRigidbody.transform;

        if (Check(target, self))
            return target;

        return null;
    }

    public bool Check(Transform other, Transform self = null)
    {
        // Ignore siblings if set
        if (ignoreSiblings && other.parent != null && self.parent == other.parent)
            return false;

        // Check layer mask
        if (layerMask.Contains(other.gameObject.layer) == false)
            return false;

        // Check tag mask
        if (tagMask.Count == 0 || tagMask.Contains(other.tag))
            return true;

        return false;
    }
}