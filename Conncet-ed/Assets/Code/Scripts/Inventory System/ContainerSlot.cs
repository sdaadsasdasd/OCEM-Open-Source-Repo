using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContainerSlot : Slot, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IScrollHandler
{
    public ItemContainer container;
    public TMPro.TMP_Text nameText;
    private bool hovered;
    public new void Update()
    {
        //this code works but it renders wrong because weird issue
        /*if (item != null && hovered)
        {
            nameText.text = item.getName();
            nameText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            nameText.transform.parent.gameObject.SetActive(false);
        }*/
        base.Update();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        container.BeginDrag(eventData, this);
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        container.Drag(eventData, this);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        container.EndDrag(eventData, this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        container.HoverEvent(this, true);
        this.gameObject.GetComponent<Image>().color = new Color(0.2f, 0.44f, 0.7f, 0.54f);
        container.PointerEnter(eventData, this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        container.HoverEvent(this, false);
        this.gameObject.GetComponent<Image>().color = new Color(0.2f, 0.84f, 0.7f, 0.54f);
        container.PointerExit(eventData, this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        container.Click(eventData, this);
    }
    public void OnScroll(PointerEventData eventData)
    {
        container.Scroll(eventData, this);
    }
}
