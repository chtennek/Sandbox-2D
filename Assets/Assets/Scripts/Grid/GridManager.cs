using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    private HashSet<GridBehaviour> entities = new HashSet<GridBehaviour>();

    private void Awake()
    {
        foreach (GridBehaviour entity in FindObjectsOfType<GridBehaviour>())
            entities.Add(entity);
    }
}
