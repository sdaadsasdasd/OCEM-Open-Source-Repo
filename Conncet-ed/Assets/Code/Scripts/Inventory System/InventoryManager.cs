using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public ItemContainer _mainInventory;
    public ItemContainer _hotbar;
    public CursorSlot _cursorSlot;
    public GameObject hoverText;
    private double _weight = 0;
    [SerializeField] private double WEIGHT_CAP;
    public List<Item> items = new List<Item>();
    public double getWeightCap()
    {
        return WEIGHT_CAP;
    }
    public double getWeight()
    {
        return _weight;
    }
    public List<ItemStack> getItems()
    {
        List<ItemStack> retVal = _mainInventory.getItems();
        retVal.AddRange(_hotbar.getItems());
        retVal.Add(_cursorSlot.getItem());
        return retVal;
    }
    public void recalculateWeight()
    {
        List<ItemStack> items = getItems();
        double weight = 0;
        foreach(ItemStack s in items)
        {
            weight += s.getStackSize() * s.getWeight();
            if(weight > WEIGHT_CAP)
            {
                Debug.LogWarning("You are overweight!!!!!111");
            }
        }
    }

    //This method should be used in events like OnItemPickup
    public void addItemToInventory(ItemStack item)
    {
        if (_weight + item.getTotalWeight() > WEIGHT_CAP)
        {
            Debug.LogWarning("Unable to add item " + item.ToString() + " not enough weight");
            return;
        }
        if(!_mainInventory.addItem(item))
        {
            if (!_hotbar.addItem(item))
            {
                Debug.LogWarning("Manager failed to add item: " + item.ToString());
                return;
            }
        }
        _weight += item.getTotalWeight();
    }

    //Same but for events like OnItemDrop
    public void removeItemFromInventory(ItemStack item)
    {
        if (!_mainInventory.removeItem(item))
        {
            if (!_hotbar.removeItem(item))
            {
                Debug.LogWarning("Manager failed to remove item: " + item.ToString());
                return;
            }
        }
        _weight -= item.getTotalWeight();

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

    public void Click(PointerEventData data, ContainerSlot source)
    {
        if (isNullOrEmpty(_cursorSlot.getItem()))
            _cursorSlot.removeItem();

        if (isNullOrEmpty(source.getItem()))
            source.removeItem();

        if (dragging != -1)
            return;

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
                if (isNullOrEmpty(source.getItem()) || source.getItem().getName() == _cursorSlot.getItem().getName())
                {
                    //if slot under cursor has no item/item can be merged, put from cursor into slot
                    ItemStack moved = _cursorSlot.removeItem();
                    int newSize = moved.getStackSize() + (source.getItem() == null ? 0 : source.getItem().getStackSize());
                    moved.setStackSize(newSize);
                    source.setItem(moved);
                }
                else
                {
                    //this means that there is 2 different items, swap their places
                    ItemStack moved = _cursorSlot.removeItem();
                    _cursorSlot.setItem(source.getItem());
                    source.setItem(moved);
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
        HoverEvent(source, true);
    }
    public void Scroll(PointerEventData data, ContainerSlot source)
    {
        if (isNullOrEmpty(_cursorSlot.getItem()))
            _cursorSlot.removeItem();

        if (isNullOrEmpty(source.getItem()))
            source.removeItem();

        if (dragging != -1)
            return;

        //get amount of scrolled times
        int scrollAmount = (int)data.scrollDelta.y / 6;

        //scroll up (deposit from cursor into)
        if (scrollAmount > 0)
        {
            if(!isNullOrEmpty(_cursorSlot.getItem()) && (source.getItem() == null || source.getItem().getName() == _cursorSlot.getItem().getName()))
            {
                if (_cursorSlot.getItem().getStackSize() < scrollAmount)
                {
                    scrollAmount = _cursorSlot.getItem().getStackSize();
                }
                _cursorSlot.getItem().incrementCount(-scrollAmount);
                startCount -= scrollAmount;
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
        //scroll down (take item from slot)
        else if (scrollAmount < 0 && source.getItem() != null)
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
        HoverEvent(source, true);
    }

    private List<int> dragCounts = new List<int>();
    private List<ContainerSlot> dragSlots = new List<ContainerSlot>();
    private int startCount = 0;
    private int dragging = -1;

    public void BeginDrag(PointerEventData data, ContainerSlot source)
    {
        if (dragging != -1)
        {
            return;
        }
        dragging = (int) data.button;
        startCount = _cursorSlot.getItem() != null ? _cursorSlot.getItem().getStackSize() : 0;
    }

    public void Drag(PointerEventData data, ContainerSlot source)
    {
        bool hasSlot(GameObject o)
        {
            ContainerSlot s = o.GetComponent<ContainerSlot>();
            if (s != null && !dragSlots.Contains(s))
            {
                return true;
            }
            return false;
        }
        //this finds the slot you are hovering (also returns on duplicates)
        GameObject test = data.hovered.Find(o => hasSlot(o));
        if(test == null) return;
        ContainerSlot hoverSlot = test.GetComponent<ContainerSlot>();


        if (isNullOrEmpty(_cursorSlot.getItem()))
        {
            _cursorSlot.removeItem();
            return;
        }
        if (isNullOrEmpty(hoverSlot.getItem()))
            hoverSlot.removeItem();

        //this makes sure that you cant do 2 drags at the same time
        if ((int) data.button != dragging)
            return;

        if (canStack(_cursorSlot.getItem(), hoverSlot.getItem()) && startCount > 0)
        {
            // 2 lists seems like not the best idea to me but i don't know how to do it in c#
            dragSlots.Add(hoverSlot);
            dragCounts.Add(hoverSlot.getItem() != null ? hoverSlot.getItem().getStackSize() : 0);

            //this is how much of item should be in each slot
            int split = data.button == PointerEventData.InputButton.Left ? startCount / (dragSlots.Count + 1) : 1;
            int remain = startCount;
            for (int i = 0; i < dragSlots.Count; i++)
            {
                //put the items in slots
                dragSlots[i].setItem(_cursorSlot.getItem().copy());
                dragSlots[i].getItem().setStackSize(dragCounts[i] + split);
                remain -= split;

                if (isNullOrEmpty(dragSlots[i].getItem()))
                    dragSlots[i].removeItem();
            }
            //put the remaining items in cursor
            _cursorSlot.getItem().setStackSize(remain);
        }

        if (isNullOrEmpty(_cursorSlot.getItem()))
            _cursorSlot.removeItem();

        if (isNullOrEmpty(source.getItem()))
            source.removeItem();
    }
    public void EndDrag(PointerEventData data, ContainerSlot source)
    {
        dragSlots.Clear();
        dragCounts.Clear();
        dragging = -1;
    }

    public void AddRandomItem()
    {
        ItemStack s = new ItemStack(items[UnityEngine.Random.Range(0, items.Count)], UnityEngine.Random.Range(1, 100));
        if(_weight + s.getTotalWeight() > WEIGHT_CAP)
        {
            //horribly unoptimized code to "make the stack fit"
            for(int i = s.getStackSize(); i>0; i--)
            {
                if(_weight + i * s.getWeight() <= WEIGHT_CAP)
                {
                    s.setStackSize(i);
                    break;
                }
            }
        }
        addItemToInventory(s);
    }

    internal void HoverEvent(ContainerSlot source, bool hide)
    {
        if (source.getItem() != null)
        {
            hoverText.GetComponentInChildren<TMPro.TMP_Text>().text = source.getItem().getName();
            hoverText.transform.position = source.transform.position;
            hoverText.SetActive(true);
        }
        else
        {
            hoverText.SetActive(false);
        }
    }

    public void Sort()
    {
        _mainInventory.sortInventory();
    }
}

