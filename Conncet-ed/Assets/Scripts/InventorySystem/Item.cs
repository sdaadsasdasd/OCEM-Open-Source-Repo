using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Items/BaseItem", order = 1)]
public class Item : ScriptableObject
{
    //Attributes
    private int _id;
    public string _name;
    public int _stackSize;
    public double _weight;
    public Category _category;

    public string getName()
    {
        return _name;
    }

    public int getStackSize()
    {
        return _stackSize;
    }

    public double getWeight()
    {
        return _weight;
    }

    public Category getCategory()
    {
        return _category;
    }

    public virtual void Use()
    {

    }

}
