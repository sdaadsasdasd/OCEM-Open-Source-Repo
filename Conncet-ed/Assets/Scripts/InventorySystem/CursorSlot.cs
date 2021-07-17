using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSlot : Slot
{
    public Item defaultItem;
    public void Start()
    {
        item = new ItemStack(defaultItem, 25);
    }
    public void OnUICursor(InputValue value)
    {
        //(this.gameObject.transform as RectTransform).position = value.Get<Vector2>();
        //Debug.Log(value.Get<Vector2>());
    }
}
