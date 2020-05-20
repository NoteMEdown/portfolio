using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class doubleClick : MonoBehaviour, IPointerClickHandler
{
    private int tap;
    private float interval = 0.4f;
    private float deltaTime;
    private Client client;
    [SerializeField]
    private mapPiece map;

    public void OnPointerClick(PointerEventData eventData)
    {
        try
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                client.SetCurrentMapPiece(this.gameObject.GetComponent<mapPiece>());
            else
                client.setMapInfo(map.DataInfo());
        }
        catch (Exception e) { }
        
       
    }

    public void setClient(Client c)
    {
        client = c;
    }

    
}