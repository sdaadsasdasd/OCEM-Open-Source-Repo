using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{

    bool is_player_close = false;

    public GameObject item_1;
    public GameObject item_2;
    public GameObject result;

    GameObject crafting_menu;

    private void Awake()
    {
        crafting_menu = GameObject.Find("/Canvas/Crafting Menu");

        item_1 = null;
        item_2 = null;
        result = null;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (is_player_close)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (crafting_menu.GetComponent<CraftingMenu>().IsMenuOpen() == false)
                    OpenCraftingMenu();
                else
                    CloseCraftingMenu();
                    
            }
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.layer == 3)
        {
            is_player_close = true;
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.gameObject.layer == 3)
        {
            is_player_close = false;
            CloseCraftingMenu();
        }
    }

    private void OpenCraftingMenu()
    {
        crafting_menu.GetComponent<CraftingMenu>().OpenMenu();
    }

    private void CloseCraftingMenu()
    {
        crafting_menu.GetComponent<CraftingMenu>().CloseMenu();
    }

}
