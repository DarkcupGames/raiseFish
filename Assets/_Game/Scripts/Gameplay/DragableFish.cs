using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableFish : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    Vector3 startDrag;
    Vector3 oldPos;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position += (newPos - oldPos);
        oldPos = newPos;

        Debug.Log(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        oldPos = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(eventData.position);
    }
}
