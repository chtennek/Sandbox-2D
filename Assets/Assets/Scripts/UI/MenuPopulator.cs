using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class MenuPopulator<D, T> : MonoBehaviour where D : IDisplayer<T>
{
    public GameMenu menu;
    public bool preserveChildrenWithLayoutElements = true;
    public Transform menuItemParent;
    public Transform menuItemPrefab;

    public bool populateOnAwake = false;

    private List<D> displays = new List<D>();

    private void Reset()
    {
        menu = GetComponent<GameMenu>();
        menuItemParent = transform;
    }

    private void Awake()
    {
        if (populateOnAwake)
            PopulateMenu(displays);
    }

    public void ClearMenu()
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

    public void PopulateMenu(IEnumerable<T> datas)
    {
        ClearMenu();
        foreach (T data in datas)
            AddMenuItem(data);
    }

    public void AddMenuItem(T data)
    {
        Transform obj = ObjectPooler.Instantiate(menuItemPrefab, menuItemParent);
        if (obj == null)
            return;

        D displayer = obj.GetComponent<D>();
        if (displayer == null)
            return;

        displayer.Display(data);
    }
}