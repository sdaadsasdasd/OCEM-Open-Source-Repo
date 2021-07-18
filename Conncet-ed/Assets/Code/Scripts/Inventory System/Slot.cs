using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour
{
    private int id;
    protected ItemStack item;
    public Image img;
    public TMPro.TMP_Text text;

    [SerializeField] private Item startItem = null;
    [SerializeField] private int startItemCount = 0;

    public void Update()
    {
        if(item != null)
        {
            img.sprite = item.getIcon();
            img.color = Color.white;
            text.text = item.getStackSize().ToString();
        }
        else
        {
            img.color = Color.clear;
            img.sprite = null;
            text.text = "";
        }
    }
    public void Start()
    {
        if(startItem != null && startItemCount > 0)
        {
            this.setItem(new ItemStack(startItem, startItemCount));
        }
    }

    public ItemStack getItem()
    {
        return item;
    }
    public ItemStack removeItem()
    {
        ItemStack retVal = item;
        item = null;
        return retVal;
    }

    public void setItem(ItemStack item)
    {
        this.item = item;
    }
}
