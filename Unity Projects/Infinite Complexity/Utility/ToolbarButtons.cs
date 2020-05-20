using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarButtons : MonoBehaviour {
    
    private Client Client;
    [SerializeField]
    private GameObject guide;

	// Use this for initialization
	void Start () {
        Client = GameObject.FindGameObjectWithTag("Server").GetComponent<Client>();

	}

    public void minToggle(string targetName)
    {
        switch (targetName)
        {
            case "map":
                Client.minBasicToggle("map");
                break;
            case "terminal":
                Client.minTerminalToggle();
                break;
        }
    }

    public void newMinToggle(string target)
    {
        GameObject go = null;
        switch (target)
        {
            case "guide":
                go = guide;
                break;
        }
        try
        {
            go.gameObject.SetActive(!go.gameObject.activeSelf);
        }
        catch (System.Exception e) { Debug.Log(e); }
        
    }
}
