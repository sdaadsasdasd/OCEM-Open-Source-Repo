using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowResize : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    //a reference to the panel that should be resized
    [SerializeField] private GameObject panelRoot;

    //since the same script is applied to every button
    //we need a way to know which button is right,left,top,bottom,topright etc.
    [SerializeField] private Vector2 dragDir;

    //cursor textures, idk if this is a good way of doing this
    [SerializeField] private Texture2D curEW;
    [SerializeField] private Texture2D curNESW;
    [SerializeField] private Texture2D curNS;
    [SerializeField] private Texture2D curNWSE;

    //variables for storing the initial state of the panel when dragged
    private Vector2 startOffsetMin;
    private Vector2 startOffsetMax;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //store the initial state when user starts dragging
        startOffsetMin = (panelRoot.transform as RectTransform).offsetMin;
        startOffsetMax = (panelRoot.transform as RectTransform).offsetMax;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //we have to use local variables to change the x and y coordinates individually and later set them to the actual panels offsets
        Vector2 offsetMin = startOffsetMin;
        Vector2 offsetMax = startOffsetMax;

        //this thing calculates how much the user has dragged from the start including scaling and clamping the cursor to the edges of the screen
        Vector2 scaledEventPos = (new Vector2(Mathf.Clamp(eventData.position.x, 0, Screen.width), Mathf.Clamp(eventData.position.y, 0, Screen.height)) - eventData.pressPosition) / panelRoot.GetComponentInParent<Canvas>().scaleFactor;

        //this is a bunch of tests to see which axis to scale the panel on
        if (dragDir.x > 0)
        {
            offsetMax.x = scaledEventPos.x + startOffsetMax.x;
        }
        else if (dragDir.x < 0)
        {
            offsetMin.x = scaledEventPos.x + startOffsetMin.x;
        }
        if (dragDir.y > 0)
        {
            offsetMax.y = scaledEventPos.y + startOffsetMax.y;
        }
        else if (dragDir.y < 0)
        {
            offsetMin.y = scaledEventPos.y + startOffsetMin.y;
        }

        //it took me way too long to figure out how to calculate the width/height of the panel from its offset min and max
        //all it takes is subtracting offsetMin from offsetMax....
        if (offsetMax.x - offsetMin.x < 300)
        {
            offsetMin.x = (panelRoot.transform as RectTransform).offsetMin.x;
            offsetMax.x = (panelRoot.transform as RectTransform).offsetMax.x;
        }
        if(offsetMax.y - offsetMin.y  < 250)
        {
            offsetMin.y = (panelRoot.transform as RectTransform).offsetMin.y;
            offsetMax.y = (panelRoot.transform as RectTransform).offsetMax.y;
        }

        //apply the offsets again after clamping them
        (panelRoot.transform as RectTransform).offsetMin = offsetMin;
        (panelRoot.transform as RectTransform).offsetMax = offsetMax;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(dragDir.x != 0 && dragDir.y == 0)
            Cursor.SetCursor(curEW, new Vector2(32,32), CursorMode.Auto);
        else if(dragDir.x == 0 && dragDir.y != 0)
            Cursor.SetCursor(curNS, new Vector2(32, 32), CursorMode.Auto);
        else if(Mathf.Sign(dragDir.x) == Mathf.Sign(dragDir.y))
            Cursor.SetCursor(curNESW, new Vector2(32,32), CursorMode.Auto);
        else
            Cursor.SetCursor(curNWSE, new Vector2(32, 32), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, new Vector2(), CursorMode.Auto);
    }
}
