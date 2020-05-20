using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouseOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    private Client client;

    public void Start()
    {
        client = GameObject.FindGameObjectWithTag("Server").GetComponent<Client>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        client.SetMouseOptionLocation(this.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (client.GetMouseOptionLocation().Equals(this.name))
            client.SetMouseOptionLocation("Desktop");
    }
}
