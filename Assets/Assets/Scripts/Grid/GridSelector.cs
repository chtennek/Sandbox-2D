using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GridSelector : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool useMouse;
    [SerializeField] private CompoundMask mask;

    [Header("References")]
    [SerializeField] private GridSettings grid;

    public Int3UnityEvent onHighlight;
    public Int3UnityEvent onSelect;

    private void Reset()
    {
        grid = GetComponent<GridSettings>();
    }

    private void Update()
    {
        if (useMouse)
        {
            List<Vector3> targets = mask.Mousecast();
            if (targets.Count > 0)
            {
                Vector3 target = targets[0];
                Vector3Int point = grid.ToGridSpace(target);

                onHighlight.Invoke(point.x, point.y, point.z);
                if (Input.GetMouseButtonDown(0))
                    onSelect.Invoke(point.x, point.y, point.z);
            }
        }
    }
}
