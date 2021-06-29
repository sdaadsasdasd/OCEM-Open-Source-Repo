using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> _items;

    //This method should be used in events like OnItemPickup
    public void addItemToInventory(Item item)
    {
        _items.Add(item);
    }

    //Same but for events like OnItemDrop
    public void removeItemFromInventory(Item item)
    {
        _items.Remove(item);
    }

}

