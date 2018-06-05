using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TransformSelector : MonoBehaviour, IComparer<Transform>
{
    [SerializeField]
    private string comment;

    [Header("Input Type")]
    [SerializeField]
    private bool useMouse;

    [SerializeField]
    private int mouseButton;

    [Header("Properties")]
    [SerializeField]
    private CompoundMask mask;

    [SerializeField]
    private float range = 10;

    [SerializeField]
    private float mouseRaycast = 1000;

    [SerializeField]
    private TransformUnityEvent onSelect;

    private Transform lastSelected;
    private List<Transform> targetCache;

    private void Update()
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            Select(GetMouseTarget());
        }
    }

    private void Select(Transform target)
    {
        if (target != null)
            return;
        onSelect.Invoke(target);
        lastSelected = target;
    }

    public void SelectPreviousTarget()
    {
        Select(FindNextTarget(-1));
    }

    public void SelectNextTarget()
    {
        Select(FindNextTarget(1));
    }

    private Transform FindNextTarget(int offset = 1)
    {
        if (targetCache.Count == 0 || lastSelected == null)
            return null;

        int indexOf = targetCache.IndexOf(lastSelected);
        if (indexOf == -1)
            return null;

        return targetCache[(indexOf + offset) % targetCache.Count];
    }

    public void SelectNearestTarget()
    {
        Select(FindNearestTarget());
    }

    private Transform FindNearestTarget()
    {
        List<Transform> candidates = mask.GetCollidersWithin(range, transform);
        if (candidates.Count == 0)
            return null;

        candidates.Sort(this);
        targetCache = candidates;
        return candidates[0];
    }

    private Transform GetMouseTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, mouseRaycast, mask.layerMask) == false)
            return null;

        if (mask.tagMask.Count > 0 && mask.tagMask.Contains(hit.transform.tag) == false)
            return null;

        return hit.transform;
    }

    public int Compare(Transform x, Transform y)
    {
        return Vector3.Distance(x.position, transform.position).CompareTo(Vector3.Distance(y.position, transform.position));
    }
}
