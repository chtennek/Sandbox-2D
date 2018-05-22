using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UINavigator : MonoBehaviour, INavigator
{
    [Header("Input")]
    public InputReceiver input;
    public string axisPairName = "UI";
    public string submitButton = "UISubmit";
    public string cancelButton = "UICancel";

    public string closeActiveButton = "UICancel";
    public string closeAllButton = "UIStart"; // Close all parent menus, not close all disjoint menus

    public StringToMenuDictionary openMenuButton = new StringToMenuDictionary();
    public StringToMenuDictionary closeMenuButton = new StringToMenuDictionary();

    [Header("Parameters")]
    public bool pauseOnActive = true;
    public bool lockInputOnActive = true;
    public bool moveOneElementPerInput = true;
    public int inputsPerSecond = 10;
    //public float repeatDelay = 0.1f;

    private float m_lastActionTime = 0f;
    private Vector2 m_lastDirection = Vector2.zero;

    [Header("References")]

    private InGameMenu m_ActiveMenu;
    public InGameMenu ActiveMenu
    {
        get { return m_ActiveMenu; }
        set
        {
            m_ActiveMenu = value;
            if (m_ActiveMenu != null)
            {
                if (lockInputOnActive) input.Lock();
                if (pauseOnActive) Time.timeScale = 0; // [TODO] doesn't work if another UINavigator exists
            }
            else
            {
                input.Unlock();
                Time.timeScale = 1;
            }
        }
    }

    public UICursor cursor;
    private Selectable m_Selected;
    public Selectable Selected
    {
        get { return m_Selected; }
        set
        {
            BaseEventData data = new BaseEventData(null);
            if (m_Selected != null)
                m_Selected.OnDeselect(data); // [TODO] doesn't work if another UINavigator exists

            m_Selected = value;
            if (m_Selected != null)
                m_Selected.OnSelect(data);

            if (cursor != null)
                if (m_Selected != null)
                    cursor.Target = m_Selected.transform;
                else
                    cursor.Target = null;
        }
    }

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        if (input == null)
            Warnings.ComponentMissing(this);
    }

    private void Start()
    {
        StartCoroutine(Coroutine_UILoop());
    }

    private IEnumerator Coroutine_UILoop()
    {
        while (true)
        {
            yield return null;
            if (input == null)
                continue;

            bool closedMenuThisFrame = ActiveMenu != null && (input.GetButtonDown(closeActiveButton) || input.GetButtonDown(closeAllButton));

            // Movement
            if (Selected != null)
                ProcessMovement();

            // Close menus
            if (input.GetButtonDown(closeActiveButton))
                MenuUpOneLevel();
            if (input.GetButtonDown(closeAllButton))
                while (ActiveMenu != null)
                    MenuUpOneLevel();

            foreach (string buttonName in closeMenuButton.Keys)
            {
                if (input.GetButtonDown(buttonName) && closeMenuButton[buttonName].Enabled == true)
                {
                    closedMenuThisFrame = true;
                    MenuClose(closeMenuButton[buttonName]);
                }
            }

            // Open menus
            if (closedMenuThisFrame == false)
            {
                foreach (string buttonName in openMenuButton.Keys)
                {
                    if (input.GetButtonDown(buttonName))
                    {
                        MenuOpen(openMenuButton[buttonName]);
                    }
                }
            }

            // Propagate events
            if (input.GetButtonDown(submitButton))
            {
                ISubmitHandler handler = Selected as ISubmitHandler;
                if (handler != null)
                    handler.OnSubmit(null);
            }
            if (input.GetButtonDown(cancelButton))
            {
                ICancelHandler handler = Selected as ICancelHandler;
                if (handler != null)
                    handler.OnCancel(null);
            }
        }
    }

    private void ProcessMovement()
    {
        float time = Time.unscaledTime;
        Vector2 direction = input.GetAxisPair(axisPairName).LargestAxis().Quantized();

        // Figure out if we should move
        bool processMovement = Time.unscaledTime - m_lastActionTime >= 1f / inputsPerSecond;
        if (moveOneElementPerInput == true && direction == m_lastDirection)
            processMovement = false;

        // Move cursor
        if (processMovement == true && direction != Vector2.zero)
        {
            Selectable target = Selected.FindSelectable(direction);
            if (target != null)
                Selected = target;

            m_lastActionTime = Time.unscaledTime;
        }
        m_lastDirection = direction;
    }

    public void MenuCloseActive() { MenuClose(ActiveMenu); }
    public void MenuClose(InGameMenu menu)
    {
        if (menu == null)
            return;
        if (ActiveMenu == menu)
        {
            ActiveMenu = null;
            Selected = null;
        }
        menu.Disable();
    }

    public void MenuOpen(InGameMenu menu)
    {
        if (menu == null)
            return;
        menu.Enable();
        Selected = menu.FirstSelected;
        ActiveMenu = menu;

        if (cursor != null) // [TODO] refactor UICursor
            cursor.transform.position = Selected.transform.position;
    }

    public void MenuToggle(InGameMenu menu)
    {
        if (menu == null)
            return;
        if (ActiveMenu == menu)
            MenuClose(menu);
        else
            MenuOpen(menu);
    }

    public void MenuSwitch(InGameMenu menu)
    {
        MenuClose(ActiveMenu);
        MenuOpen(menu);
    }

    public void MenuUpOneLevel()
    {
        if (ActiveMenu != null)
            MenuSwitch(ActiveMenu.parentMenu);
    }

    public void Select(Selectable item)
    {
        Selected = item;
    }
}
