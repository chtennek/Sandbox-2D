using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private WaypointControl wm;
    private Canvas canvas;

    private void Awake()
    {
        wm = GetComponent<WaypointControl>();
        canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }

    public void OnSelected()
    {
        if (canvas != null)
        {
            canvas.enabled = true;
        }
    }

    public void OnDeselected()
    {
        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }
}
