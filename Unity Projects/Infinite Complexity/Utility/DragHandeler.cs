using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandeler : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler{

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    [SerializeField]
    private GameObject objectToDrag, windows, empty;

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = objectToDrag;
        itemBeingDragged.transform.SetParent(empty.transform);
        itemBeingDragged.transform.SetParent(windows.transform);
        //        (Change pivot if possible?)
        //      Debug.Log(itemBeingDragged.name);
        startPosition = Input.mousePosition - itemBeingDragged.transform.position;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //      itemBeingDragged.transform.Translate(Input.mousePosition - itemBeingDragged.transform.position); // These two lines seem to do the same thing?
        itemBeingDragged.transform.position = Input.mousePosition- startPosition; // might mess with this.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        Cursor.lockState = CursorLockMode.None;


    }
}
