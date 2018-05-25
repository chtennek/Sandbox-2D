using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UINavigator : MonoBehaviour, INavigator
{
    private bool isTransitioning = false;

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
    public bool lockInput = true;
    public bool moveOneElementPerInput = true;
    public int inputsPerSecond = 10;
    //public float repeatDelay = 0.1f;

    private float m_lastActionTime = 0f;
    private Vector2 m_lastDirection = Vector2.zero;

    [Header("References")]
    public GameMenu activeMenu;

    private Selectable GetHighlighted() {
        if (activeMenu == null || activeMenu.cursor == null)
            return null;
        return activeMenu.cursor.Highlighted;
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

    private void Update()
    {
        if (isTransitioning || input == null)
            return;

        bool closedMenuThisFrame = activeMenu != null && (input.GetButtonDown(closeActiveButton) || input.GetButtonDown(closeAllButton));

        // Cursor movement
        ProcessMovement();

        // Close menus
        if (input.GetButtonDown(closeActiveButton))
            MenuUpOneLevel();
        if (input.GetButtonDown(closeAllButton))
            while (activeMenu != null)
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

        // Handle submit
        if (input.GetButtonDown(submitButton))
        {
            ISubmitHandler handler = GetHighlighted() as ISubmitHandler;
            if (handler != null)
                handler.OnSubmit(null);

            if (activeMenu != null && activeMenu.closeMenuOnSubmit)
                MenuCloseActive();
        }

        // Handle cancel
        if (input.GetButtonDown(cancelButton))
        {
            ICancelHandler handler = GetHighlighted() as ICancelHandler;
            if (handler != null)
                handler.OnCancel(null);

            if (activeMenu != null && activeMenu.closeMenuOnCancel)
                MenuCloseActive();
        }
    }

    private void ProcessMovement()
    {
        if (activeMenu == null || activeMenu.cursor == null)
            return;

        float time = Time.unscaledTime;
        Vector2 direction = input.GetAxisPair(axisPairName).LargestAxis().Quantized();

        // Figure out if we should move
        bool processMovement = Time.unscaledTime - m_lastActionTime >= 1f / inputsPerSecond;
        if (moveOneElementPerInput == true && direction == m_lastDirection)
            processMovement = false;

        // Move cursor
        if (processMovement == true && direction != Vector2.zero)
        {
            activeMenu.cursor.Move(direction);
            m_lastActionTime = Time.unscaledTime;
        }
        m_lastDirection = direction;
    }

    public void MenuCloseActive() { MenuClose(activeMenu); }
    public void MenuClose(GameMenu menu)
    {
        IEnumerator coroutine = Coroutine_SetMenu(menu, false);
        StartCoroutine(coroutine);
    }

    public void MenuOpen(GameMenu menu)
    {
        Debug.Log(menu);
        IEnumerator coroutine = Coroutine_SetMenu(menu, true);
        StartCoroutine(coroutine);
    }

    private IEnumerator Coroutine_SetMenu(GameMenu menu, bool menuEnabled)
    {
        if (menu == null)
            yield break;

        // If we can't update menu.Enabled, break
        IEnumerator coroutineMenu = menu.SetEnabled(menuEnabled);
        if (coroutineMenu == null)
            yield break;

        isTransitioning = true;

        // Wait for menu animations/handling
        yield return coroutineMenu;

        if (menuEnabled) // Open menu
            activeMenu = menu;
        else if (activeMenu == menu) // Close menu
            activeMenu = null;

        isTransitioning = false;
    }

    public void MenuToggle(GameMenu menu)
    {
        if (menu == null)
            return;
        if (activeMenu == menu)
            MenuClose(menu);
        else
            MenuOpen(menu);
    }

    public void MenuSwitch(GameMenu menu)
    {
        MenuClose(activeMenu);
        MenuOpen(menu);
    }

    public void MenuUpOneLevel()
    {
        if (activeMenu != null)
            MenuSwitch(activeMenu.parentMenu);
    }
}
