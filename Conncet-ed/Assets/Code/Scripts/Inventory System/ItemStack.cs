using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : System.IComparable<ItemStack>
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

    public override string ToString()
    {
        return amount + "x" + type.name;
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
    public Item getItem()
    {
        return type;
    }
    public double getTotalWeight()
    {
        return type.getWeight() * amount;
    }
    public double getWeight()
    {
        return type.getWeight();
    }

    public int CompareTo(ItemStack other)
    {
        return getName().CompareTo(other.getName());
    }
}
