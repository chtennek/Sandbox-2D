using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuPopulator : MonoBehaviour
{
    public GameMenu menu;
    public bool preserveChildrenWithLayoutElements = true;
    public Transform menuItemParent;
    public Transform menuItemPrefab;

    public StringUnityEvent onSubmit;

    public bool populateOnAwake = false;
    private List<string> selections = new List<string>();

    private void Reset()
    {
        menu = GetComponent<GameMenu>();
        menuItemParent = transform;
    }

    private void Awake()
    {
        if (populateOnAwake)
            PopulateMenu(selections);
    }

    public virtual List<string> GetMenuItems()
    {
        return selections;
    }

    public void ClearMenu()
    {
        // Keep the menu cursor around
        if (menu.cursor != null)
            menu.cursor.Highlighted = null;

        selections = new List<string>();

        // Destroy current menu items
        List<Transform> children = new List<Transform>();

        foreach (Transform child in menuItemParent)
            if (preserveChildrenWithLayoutElements == false || child.GetComponent<LayoutElement>() == null)
                children.Add(child);

        foreach (Transform child in menuItemParent)
            ObjectPooler.Destroy(child);
    }

    public void AddMenuItem(string selection)
    {
        selections.Add(selection);

        Transform obj = ObjectPooler.Instantiate(menuItemPrefab);
        if (obj == null)
            return;

        Vector3 scale = obj.localScale;
        obj.SetParent(menuItemParent);
        obj.localScale = scale;

        Text text = obj.GetComponentInChildren<Text>();
        if (text != null)
            text.text = selection;

        Button button = obj.GetComponentInChildren<Button>();
        if (button != null)
            button.onClick.AddListener(delegate { onSubmit.Invoke(selection); });
    }

    public void PopulateMenu(IEnumerable selections)
    {
        ClearMenu();
        foreach (string selection in selections)
            AddMenuItem(selection);
    }
}