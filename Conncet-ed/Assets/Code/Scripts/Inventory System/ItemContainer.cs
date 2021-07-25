using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemContainer : MonoBehaviour
{
    public InventoryManager manager;
    public Vector2 gridSize;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject slotPrefab;

    protected List<ContainerSlot> slots = new List<ContainerSlot>();
    public void Start()
    {
        RectTransform rect = transform as RectTransform;
        rect.sizeDelta = gridSize * 40;
        for(int i = 0; i < gridSize.y; i++)
        {
            GameObject row = Instantiate(rowPrefab, transform);
            row.name = "Row_" + i;
            for(int j = 0; j < gridSize.x; j++)
            {
                GameObject slot = Instantiate(slotPrefab, row.transform);
                slot.name = "Slot_" + (i * gridSize.x + j);
                ContainerSlot cont = slot.GetComponent<ContainerSlot>();
                cont.container = this;
                slots.Add(cont);
            }
        }
    }
    public List<ItemStack> getItems()
    {
        List<ItemStack> retVal = new List<ItemStack>();
        foreach(Slot s in slots)
        {
            retVal.Add(s.getItem());
        }
        return retVal;
    }

    public void HoverEvent(ContainerSlot source, bool hide)
    {
        manager.HoverEvent(source, hide);
    }
    public void Subscribe(ContainerSlot s)
    {
        slots.Add(s);
    }

    public void BeginDrag(PointerEventData data, ContainerSlot source)
    {
        manager.BeginDrag(data, source);
    }
    public void Drag(PointerEventData data, ContainerSlot source)
    {
        manager.Drag(data, source);
    }
    public void EndDrag(PointerEventData data, ContainerSlot source)
    {
        manager.EndDrag(data, source);
    }
    public void PointerEnter(PointerEventData data, ContainerSlot source)
    {
        
    }
    public void PointerExit(PointerEventData data, ContainerSlot source)
    {

    }
    public void Click(PointerEventData data, ContainerSlot source)
    {
        /*
        Test code used to show slot order easily

        Item a = source.getItem().getItem();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].setItem(new ItemStack(a, i));
        }
        
         */

        manager.Click(data, source);
    }
    public void Scroll(PointerEventData data, ContainerSlot source)
    {
        manager.Scroll(data, source);
    }

    public void groupStacks()
    {
        foreach (ContainerSlot s in slots)
        {
            if (s.getItem() != null)
            {
                foreach (ContainerSlot x in slots)
                {
                    if (x.getItem() != null && x != s && x.getItem().getName() == s.getItem().getName())
                    {
                        s.getItem().incrementCount(x.removeItem().getStackSize());
                    }
                }
            }
        }
    }

    public void sortInventory()
    {
        groupStacks();
        List<ItemStack> items = new List<ItemStack>();
        foreach(ContainerSlot s in slots)
        {
            if(s.getItem() != null)
            {
                items.Add(s.getItem());
            }
        }
        items.Sort();
        for (int i = 0; i < slots.Count; i++)
        {
            if(items.Count > i)
            {
                slots[i].setItem(items[i]);
            }
            else
            {
                slots[i].removeItem();
            }
        }
    }

    public ContainerSlot getFirstMatchingSlot(ItemStack item, bool strict = false)
    {
        foreach(ContainerSlot s in slots)
        {
            if (strict)
            { 
                if (s.getItem() != null && s.getItem().getName() == item.getName())
                {
                    return s;
                }
            }
            else
            {
                if (manager.canStack(s.getItem(), item))
                {
                    return s;
                }
            }
        }
        return null;
    }   

    public bool removeItem(ItemStack item)
    {
        ContainerSlot s = getFirstMatchingSlot(item, true);
        if (s != null)
        {
            if (s.getItem() != null)
            {
                if(s.getItem().getStackSize() > item.getStackSize())
                {
                    s.getItem().incrementCount(-item.getStackSize());
                }
                else
                {
                    item.setStackSize(s.getItem().getStackSize());
                    s.removeItem();
                    removeItem(item);
                }
            }
            else
            {
                Debug.LogWarning("Tried to remove item " + item.ToString() + " but failed");
                return false;
            }
            return true;
        }
        Debug.LogWarning("Tried to remove item " + item.ToString() + " but failed");
        return false;
    }

    public bool addItem(ItemStack item)
    {
        ContainerSlot s = getFirstMatchingSlot(item, true);
        if(s == null)
        {
            s = getFirstMatchingSlot(item);
        }
        if (s != null)
        {
            if(s.getItem() != null)
            {
                s.getItem().incrementCount(item.getStackSize());
            }
            else
            {
                s.setItem(item);
            }
            return true;
        }
        Debug.LogWarning("Tried to add item " + item.ToString() + " but failed");
        return false;
    }
}
