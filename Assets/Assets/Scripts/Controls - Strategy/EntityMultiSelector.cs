using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

[RequireComponent(typeof(RectTransform))] // This script should be attached to a canvas element.
public class EntityMultiSelector : MonoBehaviour
{
    public LayerMask selectableMask;
    public float minDragSelectViewportDistance = .01f;

    private Vector2 selectionOrigin;

    private Mouse mouse;
    private Keyboard keyboard;
    private RectTransform rt;
    private List<EntityManager> selectedObjects = new List<EntityManager>();

    public List<EntityManager> GetSelectedObjects()
    {
        return selectedObjects;
    }

    private void Awake()
    {
        mouse = ReInput.controllers.Mouse;
        keyboard = ReInput.controllers.Keyboard;

        rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
    }

    private void Update()
    {
        if (keyboard.GetModifierKey(ModifierKey.Control))
        {
            if (mouse.GetButtonDown(0))
            {
                SelectAll();
            }
        }
        else if (mouse.GetButtonDown(0))
        {
            selectionOrigin = Camera.main.ScreenToViewportPoint(mouse.screenPosition);
            rt.anchorMin = selectionOrigin;
            rt.anchorMax = selectionOrigin;
        }
        else if (mouse.GetButtonUp(0))
        {
            if (Vector2.Distance(rt.anchorMin, rt.anchorMax) >= minDragSelectViewportDistance)
            {
                ProcessCurrentSelection();
            }
            else
            {
                SelectUnderCursor();
            }
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
        }
        else if (mouse.GetButton(0))
        {
            Vector2 currentPosition = Camera.main.ScreenToViewportPoint(mouse.screenPosition);
            rt.anchorMin = new Vector2(Mathf.Min(selectionOrigin.x, currentPosition.x), Mathf.Min(selectionOrigin.y, currentPosition.y));
            rt.anchorMax = new Vector2(Mathf.Max(selectionOrigin.x, currentPosition.x), Mathf.Max(selectionOrigin.y, currentPosition.y));
        }
    }

    private void DeselectAll()
    {
        foreach (EntityManager entity in selectedObjects)
        {
            entity.OnDeselected();
        }
        selectedObjects.Clear();
    }

    private void SelectAll()
    {
        DeselectAll();
        foreach (EntityManager entity in Resources.FindObjectsOfTypeAll<EntityManager>())
        {
            entity.OnSelected();
            selectedObjects.Add(entity);
        }
    }

    private void SelectUnderCursor()
    {
        DeselectAll();
        Collider2D[] hits = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(mouse.screenPosition));
        foreach (Collider2D c in hits)
        {
            EntityManager entity = c.GetComponent<EntityManager>();
            if (entity != null)
            {
                entity.OnSelected();
                selectedObjects.Add(entity);
            }
        }
    }

    private void ProcessCurrentSelection()
    {
        DeselectAll();
        Vector3 min = Camera.main.ViewportToWorldPoint(rt.anchorMin);
        Vector3 max = Camera.main.ViewportToWorldPoint(rt.anchorMax);
        Vector2 center = (min + max) / 2;
        Vector2 size = max - min;
        Bounds selection = new Bounds(center, size);
        Collider2D[] hits = Physics2D.OverlapAreaAll(min, max, selectableMask);
        foreach (Collider2D c in hits)
        {
            EntityManager entity = c.GetComponent<EntityManager>();
            // Don't include entity if its center isn't in bounds
            if (entity != null && selection.Contains(entity.transform.position))
            {
                entity.OnSelected();
                selectedObjects.Add(entity);
            }
        }
    }
}
