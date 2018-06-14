using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private CompoundMask mask;

    [Header("References")]
    [SerializeField] private GridSettings grid;
    [SerializeField] private PathBuilder pathing;

    private void Reset()
    {
        pathing = GetComponent<PathBuilder>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<Vector3> targets = mask.Mousecast();
            if (targets.Count > 0)
            {
                Vector3 target = targets[0];
                Vector3Int point = grid.ToGridSpace(target);
                pathing.Clear();
                pathing.enabled = !pathing.enabled;
            }
        }
    }
}
