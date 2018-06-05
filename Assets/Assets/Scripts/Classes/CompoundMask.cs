using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompoundMask
{
    public LayerMask layerMask = ~0;
    public List<string> tagMask;
    public bool ignoreSiblings = false;

    public List<Transform> GetCollidersWithin(float distance, Transform self) { return GetCollidersWithin(distance, self, Vector3.zero); }
    public List<Transform> GetCollidersWithin(float distance, Transform self, Vector3 offset)
    {
        List<Transform> targets = new List<Transform>();
        Transform target = null;

        // 3D
        foreach (Collider coll in Physics.OverlapSphere(self.position + offset, distance, layerMask))
        {
            if (coll.attachedRigidbody == null)
                target = coll.transform;
            else
                target = coll.attachedRigidbody.transform;

            if (CheckAll(self, target))
                targets.Add(target);
        }

        // 2D
        foreach (Collider2D coll2D in Physics2D.OverlapCircleAll(self.position + offset, distance, layerMask))
        {
            if (coll2D.attachedRigidbody == null)
                target = coll2D.transform;
            else
                target = coll2D.attachedRigidbody.transform;

            if (CheckAll(self, target))
                targets.Add(target);
        }

        return targets;
    }

    public bool CheckAll(Transform self, Transform other)
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