using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Items/BaseItem", order = 1)]
public class Item : ScriptableObject
{
    //Attributes
    private int _id;
    [SerializeField] private string _displayName;
    [SerializeField] private double _weight;
    [SerializeField] private Category _category;
    [SerializeField] private Sprite _icon;


    public string getName()
    {
        return _displayName;
    }

    public double getWeight()
    {
        return _weight;
    }

    public Category getCategory()
    {
        return _category;
    }

    public Sprite getIcon()
    {
        return _icon;
    }

    public virtual void Use()
    {

    }

}
