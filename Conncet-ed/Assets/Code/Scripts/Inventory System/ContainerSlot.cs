using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContainerSlot : Slot, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IScrollHandler
{
    public ItemContainer container;

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
        this.gameObject.GetComponent<Image>().color = new Color(0.2f, 0.44f, 0.7f, 0.54f);
        container.PointerEnter(eventData, this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
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
    public void Start()
    {
        container.Subscribe(this);
        base.Start();
    }
}
