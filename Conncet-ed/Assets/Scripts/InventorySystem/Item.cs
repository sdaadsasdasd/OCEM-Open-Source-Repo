using System.Collections;
using System.Collections.Generic;

public class Item
{
    //Attributes
    private int _id { get; set; }
    private string _name { get; set; }
    private int _stackSize { get; set; }
    private double _weight { get; set; }
    private Category _category { get; set; }

    //Constructor
    public Item(string name, int stackSize, double weight, Category category)
    {
        _name = name;
        _stackSize = stackSize;
        _weight = weight;
        _category = category;
    }
}
