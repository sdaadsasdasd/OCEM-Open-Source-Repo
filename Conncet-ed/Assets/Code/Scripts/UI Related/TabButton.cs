using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //references to ui controller for passing events and button image for changing button color
    [SerializeField] private UIController controller;
    [SerializeField] public Image image;
    
    //this class just passes events to its ui controller
    public void OnPointerClick(PointerEventData eventData)
    {
        controller.OnClick(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller.OnHoverStart(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        controller.OnHoverEnd(this);
    }

    void Start()
    {
        controller.AddSelf(this);
    }
}
