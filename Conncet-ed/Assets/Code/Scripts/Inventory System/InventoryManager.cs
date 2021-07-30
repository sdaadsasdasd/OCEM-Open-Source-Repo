using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public ItemContainer _mainInventory;
    public ItemContainer _hotbar;
    public CursorSlot _cursorSlot;
    private List<ContainerSlot> dragSlots = new List<ContainerSlot>(); 

    public List<ItemStack> getItems()
    {
        List<ItemStack> retVal = _mainInventory.getItems();
        retVal.AddRange(_hotbar.getItems());
        return retVal;
    }

    //This method should be used in events like OnItemPickup
    public void addItemToInventory(ItemStack item)
    {
        if (_mainInventory.isFull())
        {
            _hotbar.addItem(item);
        }
        else
        {
            _mainInventory.addItem(item);
        }
    }

    //Same but for events like OnItemDrop
    public void removeItemFromInventory(ItemStack item)
    {
        if (_mainInventory.isFull())
        {
            _hotbar.removeItem(item);
        }
        else
        {
            _mainInventory.removeItem(item);
        }
    }
    internal bool isNullOrEmpty(ItemStack stack)
    {
        if (stack == null)
        {
            return true;
        }
        else if (stack.getStackSize() == 0)
        {
            return true;
        }
        return false;
    }

    internal bool canStack(ItemStack a, ItemStack b)
    {
        if(a == null && b == null)
        {
            return false;
        }
        if(a != null)
        {
            if(b == null || a.getName() == b.getName())
            {
                return true;
            }
        }
        if (b != null)
        {
            if (a == null || a.getName() == b.getName())
            {
                return true;
            }
        }
        return false;
    }

    public void Click(PointerEventData data, ContainerSlot source, bool canGetItems)
    {
        if (isNullOrEmpty(_cursorSlot.getItem()))
            _cursorSlot.removeItem();

        if (isNullOrEmpty(source.getItem()))
            source.removeItem();
        if (data.button == PointerEventData.InputButton.Left)
        {
            //lmb handling
            if (isNullOrEmpty(_cursorSlot.getItem()))
            {
                //if cursor has no item
                if (!isNullOrEmpty(source.getItem()))
                {
                    //if slot has item, pick it up
                    ItemStack moved = source.removeItem();
                    _cursorSlot.setItem(moved);
                }
            }
            else
            {
                //if cursor has item
                if (canGetItems)
                {
                    if (isNullOrEmpty(source.getItem()) || source.getItem().getName() == _cursorSlot.getItem().getName())
                    {
                        //if slot under cursor has no item/item can be merged, put from cursor into slot
                        ItemStack moved = _cursorSlot.removeItem();
                        int newSize = moved.getStackSize() + (source.getItem() == null ? 0 : source.getItem().getStackSize());
                        moved.setStackSize(newSize);
                        source.setItem(moved);
                    }
                } 
            }
        }
        else if(data.button == PointerEventData.InputButton.Right)
        {
            //rmb handling
            if(isNullOrEmpty(_cursorSlot.getItem()))
            {
                if(!isNullOrEmpty(source.getItem()))
                {
                    //if no item in cursor, but item in slot grab half of the stack with the cursor
                    ItemStack moved = source.getItem().copy();
                    int amount = moved.getStackSize() / 2;
                    source.getItem().setStackSize(amount);
                    moved.setStackSize(moved.getStackSize() - amount);
                    _cursorSlot.setItem(moved);
                }
            }
            else
            {
                if (isNullOrEmpty(source.getItem()) || source.getItem().getName() == _cursorSlot.getItem().getName())
                {
                    _cursorSlot.getItem().incrementCount(-1);
                    if(!isNullOrEmpty(source.getItem()))
                    {
                        source.getItem().incrementCount(1);
                    }
                    else
                    {
                        ItemStack newIt = _cursorSlot.getItem().copy();
                        newIt.setStackSize(1);
                        source.setItem(newIt);
                    }
                }
            }
        }
        if (isNullOrEmpty(_cursorSlot.getItem()))
            _cursorSlot.removeItem();

        if (isNullOrEmpty(source.getItem()))
            source.removeItem();
    }
    public void Scroll(PointerEventData data, ContainerSlot source, bool canGetItems)
    {
        if (isNullOrEmpty(_cursorSlot.getItem()))
            _cursorSlot.removeItem();

        if (isNullOrEmpty(source.getItem()))
            source.removeItem();

        //get amount of scrolled times
        int scrollAmount = (int)data.scrollDelta.y / 6;

        //scroll up (deposit from cursor into)
        if (canGetItems)
        {
            if (scrollAmount > 0)
            {
                if(!isNullOrEmpty(_cursorSlot.getItem()) && (source.getItem() == null || source.getItem().getName() == _cursorSlot.getItem().getName()))
                {
                    if (_cursorSlot.getItem().getStackSize() < scrollAmount)
                    {
                        scrollAmount = _cursorSlot.getItem().getStackSize();
                    }
                    _cursorSlot.getItem().incrementCount(-scrollAmount);
                    if (source.getItem() != null)
                    {
                        source.getItem().incrementCount(scrollAmount);
                    }
                    else
                    {
                        ItemStack newIt = _cursorSlot.getItem().copy();
                        newIt.setStackSize(scrollAmount);
                        source.setItem(newIt);
                    }
                }
            }
        }
        //scroll down (take item from slot)
        if (scrollAmount < 0 && source.getItem() != null)
        {
            if (!isNullOrEmpty(source.getItem()) && (_cursorSlot.getItem() == null || source.getItem().getName() == _cursorSlot.getItem().getName()))
            {
                if(source.getItem().getStackSize() < -scrollAmount)
                {
                    scrollAmount = -source.getItem().getStackSize();
                }
                source.getItem().incrementCount(scrollAmount);
                if (_cursorSlot.getItem() != null)
                {
                    _cursorSlot.getItem().incrementCount(-scrollAmount);
                }
                else
                {
                    ItemStack newIt = source.getItem().copy();
                    newIt.setStackSize(-scrollAmount);
                    _cursorSlot.setItem(newIt);
                }
            }
        }
        if (isNullOrEmpty(_cursorSlot.getItem()))
            _cursorSlot.removeItem();
            
        if (isNullOrEmpty(source.getItem()))
            source.removeItem();
            
    }
    public void Drag(PointerEventData data, ContainerSlot source)
    {
        if (canStack(_cursorSlot.getItem(), source.getItem()))
        {
            if (!dragSlots.Contains(source))
            {
                dragSlots.Add(source);

            }
        }
        if (data.button == PointerEventData.InputButton.Left)
        {
            //ye whatevr
        }
        else if (data.button == PointerEventData.InputButton.Right)
        {

        }
    }
}

