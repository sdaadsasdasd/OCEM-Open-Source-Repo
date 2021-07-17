using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack
{
    private readonly Item type;
    private int amount;

    public ItemStack(Item type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }

    public ItemStack copy()
    {
        return new ItemStack(this.type, this.amount);
    }

    public void incrementCount(int amount)
    {
        this.amount += amount;
    }

    public Sprite getIcon()
    {
        return type.getIcon();
    }

    public int getStackSize()
    {
        return amount;
    }

    public void setStackSize(int amount)
    {
        this.amount = amount;
    }

    public string getName()
    {
        return type.getName();
    }

    
}
