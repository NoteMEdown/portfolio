using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoom : MonoBehaviour {

    [SerializeField]
    private GameObject mapAndBorders;

	public void zoomMap()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            mapAndBorders.transform.localScale = new Vector3(Mathf.Clamp(mapAndBorders.transform.localScale.x + Input.GetAxis("Mouse ScrollWheel"), 0.2f, 2f),
                    Mathf.Clamp(mapAndBorders.transform.localScale.y + Input.GetAxis("Mouse ScrollWheel"), 0.2f, 2f), 1);
        }
    }
}
