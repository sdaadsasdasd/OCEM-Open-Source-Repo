using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    //list of buttons which add themselves automatically
    private List<TabButton> buttons = new List<TabButton>();

    //colors for buttons
    [SerializeField] private Color defaultColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private Color hoverColor = new Color(0.6f, 0.6f, 0.6f);
    [SerializeField] private Color activeColor = new Color(0.4f, 0.4f, 0.4f);

    //list of screens to toggle on/off
    [SerializeField] private List<GameObject> screens;

    [SerializeField] private StarterAssets.StarterAssetsInputs starterAssets;

    //currently active screen index
    private int activeTab;

    //is the menu visisble
    private bool menuVisible;

    private void Start()
    {
        //disable all the screens' visuals
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
        //menu defaults closed
        CloseMenu();
    }

    //buttons will call this function to add themselves to the ui controller
    public void AddSelf(TabButton button)
    {
        buttons.Add(button);
    }

    //when a button is clicked it will call this function
    public void OnClick(TabButton from)
    {
        buttons[activeTab].image.color = defaultColor;
        activeTab = buttons.IndexOf(from);
        from.image.color = activeColor;
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
        screens[buttons[activeTab].transform.GetSiblingIndex()].SetActive(true);
    }

    //when a button is hovered it will call this function
    internal void OnHoverStart(TabButton from)
    {
        int i = buttons.IndexOf(from);
        if (i == activeTab) return;
        from.image.color = hoverColor;
    }

    internal void OnHoverEnd(TabButton from)
    {
        int i = buttons.IndexOf(from);
        if (i == activeTab) return;
        from.image.color = defaultColor;
    }

    //public methods so you can access them anywhere yay
    public void OpenMenu()
    {
        menuVisible = true;
        this.gameObject.SetActive(true);
        starterAssets.SetCursorState(false);
        starterAssets.cursorInputForLook = false;
        starterAssets.look = new Vector2();
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }
    public void CloseMenu()
    {
        menuVisible = false;
        this.gameObject.SetActive(false);
        starterAssets.SetCursorState(true);
        starterAssets.cursorInputForLook = true;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public bool IsMenuOpen()
    {
        return menuVisible;
    }
}
