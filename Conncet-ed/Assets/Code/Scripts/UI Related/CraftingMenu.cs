using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingMenu : MonoBehaviour
{
    
    //list of screens to toggle on/off
    [SerializeField] private List<GameObject> screens;

    [SerializeField] private StarterAssets.StarterAssetsInputs starterAssets;

    //currently active screen index
    private int activeTab = 0;

    //is the menu visisble
    private bool menuVisible;

    

    private void Start()
    {
        //menu defaults closed
        CloseMenu();
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
