using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class MenuPopulator<D, T> : MonoBehaviour, IDisplayer<IEnumerable<T>> where D : IDisplayer<T>
{
    public GameMenu menu;
    public bool preserveChildrenWithLayoutElements = true;
    public Transform menuItemParent;
    public Transform menuItemPrefab;

    [SerializeField]
    private bool useInitialValues;

    [SerializeField]
    private List<T> initialValues;

    private Dictionary<T, D> displayLookup = new Dictionary<T, D>();

    private void Reset()
    {
        menu = GetComponent<GameMenu>();
        menuItemParent = transform;
    }

    private void Start()
    {
        if (useInitialValues)
            Display(initialValues);
    }

    public void Refresh(IEnumerable<T> data)
    {
        foreach (T entry in data)
            if (displayLookup.ContainsKey(entry))
                displayLookup[entry].Display(entry);
            else
                Add(entry);
    }

    public void Display(IEnumerable<T> data)
    {
        Clear();
        foreach (T entry in data)
            Add(entry);
    }

    public void Clear()
    {
        // Keep the menu cursor around
        if (menu.cursor != null)
            menu.cursor.Highlighted = null;

        // Destroy current menu items
        List<Transform> children = new List<Transform>();

        foreach (Transform child in menuItemParent)
            if (preserveChildrenWithLayoutElements == false || child.GetComponent<LayoutElement>() == null)
                children.Add(child);

        foreach (Transform child in children)
            ObjectPooler.Destroy(child);
    }

    public D Add(T data)
    {
        if (displayLookup.ContainsKey(data))
        {
            Debug.LogWarning("Duplicate menu item not added.");
            return default(D);
        }

        Transform obj = ObjectPooler.Instantiate(menuItemPrefab, menuItemParent);
        if (obj == null)
            return default(D);

        D display = obj.GetComponent<D>();
        if (display == null)
            return default(D);

        displayLookup.Add(data, display);
        display.Display(data);
        return display;
    }
}