using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuPopulator : MonoBehaviour
{
    public InGameMenu menu;
    public Transform menuItemParent;
    public Transform menuItemPrefab;

    public StringUnityEvent onSelect;

    public List<string> selections;

    private void Reset()
    {
        menu = GetComponent<InGameMenu>();
        menuItemParent = transform;
    }

    private void Start()
    {
        PopulateMenu();
    }

    public virtual List<string> GetMenuItems()
    {
        return selections;
    }

    public void PopulateMenu(List<string> selections)
    {
        this.selections = selections;
        PopulateMenu();
    }

    public void PopulateMenu()
    {
        foreach (Transform child in menuItemParent)
            ObjectPooler.Destroy(child);

        foreach (string selection in selections)
        {
            Transform obj = ObjectPooler.Instantiate(menuItemPrefab);
            if (obj == null)
                continue;

            Vector3 scale = obj.localScale;
            obj.SetParent(menuItemParent);
            obj.localScale = scale;

            Text text = obj.GetComponentInChildren<Text>();
            if (text != null)
                text.text = selection;

            Button button = obj.GetComponentInChildren<Button>();
            if (button != null)
                button.onClick.AddListener(delegate
                {
                    onSelect.Invoke(selection);
                });
        }
    }
}