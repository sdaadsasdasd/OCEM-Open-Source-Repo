using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemContainer : MonoBehaviour
{
    public InventoryManager manager;

    protected List<ContainerSlot> slots = new List<ContainerSlot>();
    public List<ItemStack> getItems()
    {
        List<ItemStack> retVal = new List<ItemStack>();
        foreach(Slot s in slots)
        {
            retVal.Add(s.getItem());
        }
        return retVal;
    }

    public void Subscribe(ContainerSlot s)
    {
        slots.Add(s);
    }

    public void BeginDrag(PointerEventData data, ContainerSlot source)
    {

    }
    public void Drag(PointerEventData data, ContainerSlot source)
    {

    }
    public void EndDrag(PointerEventData data, ContainerSlot source)
    {

    }
    public void PointerEnter(PointerEventData data, ContainerSlot source)
    {
        
    }
    public void PointerExit(PointerEventData data, ContainerSlot source)
    {

    }
    public void Click(PointerEventData data, ContainerSlot source)
    {
        manager.Click(data, source);
    }
    public void Scroll(PointerEventData data, ContainerSlot source)
    {
        manager.Scroll(data, source);
    }

    internal bool isFull()
    {
        return false;
    }

    internal void removeItem(ItemStack item)
    {
        throw new NotImplementedException();
    }

    internal void addItem(ItemStack item)
    {
        throw new NotImplementedException();
    }
}
