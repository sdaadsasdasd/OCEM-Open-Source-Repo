using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    //the panel game object
    [SerializeField] private GameObject panelRoot;
    
    //drag start position, used for calculation
    private Vector3 startPos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //set the start pos when start drag
        startPos = panelRoot.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //set the panel position to the position that the cursor moved to
        //idk if there is a better way to do this since this method requires a quaternion...
        panelRoot.transform.SetPositionAndRotation(new Vector3(
            startPos.x + (Mathf.Clamp(eventData.position.x, 4, Screen.width-4) - eventData.pressPosition.x),
            startPos.y + (Mathf.Clamp(eventData.position.y, 4, Screen.height-4) - eventData.pressPosition.y)), new Quaternion());
    }
}
