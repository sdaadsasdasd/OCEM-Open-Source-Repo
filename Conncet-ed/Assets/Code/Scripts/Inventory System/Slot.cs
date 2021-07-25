using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour
{
    private int id;
    protected ItemStack item;
    public Image img;
    public TMPro.TMP_Text countText;

    public void Update()
    {
        if(item != null)
        {
            img.sprite = item.getIcon();
            img.color = Color.white;
            countText.text = item.getStackSize().ToString();
        }
        else
        {
            img.color = Color.clear;
            img.sprite = null;
            countText.text = "";
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
