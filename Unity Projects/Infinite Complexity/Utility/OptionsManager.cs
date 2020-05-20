using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour {

    [SerializeField]
    private GameObject DesktopOptions, MapOptions;


    public void CreateOption(string option)
    {
        bool validChoice = true;
        GameObject choice = DesktopOptions;
        switch (option.Split('-')[0])
        {
            case "Desktop": choice = DesktopOptions;
                break;
            case "MapPiece": choice = MapOptions;
                break;
            default: validChoice = false;
                break;
        }
        if (validChoice)
        {
            choice.SetActive(true);
            choice.transform.position = Input.mousePosition + new Vector3(1, 0, 0); // Vector is for offset, so that I am still hovering over correct obvject
        }
        
    }
}
