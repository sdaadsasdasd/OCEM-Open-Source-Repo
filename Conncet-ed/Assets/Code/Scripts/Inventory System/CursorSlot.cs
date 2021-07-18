using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSlot : Slot
{
    public void OnUICursor(InputValue value)
    {
        //TO:DO make cursor slot actually position at the cursor... yeah i dont know how to do that
        
        //(this.gameObject.transform as RectTransform).position = value.Get<Vector2>();
        //Debug.Log(value.Get<Vector2>());
    }
}
